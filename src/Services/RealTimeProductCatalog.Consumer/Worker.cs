using System.Text.Json;
using Confluent.Kafka;
using RealTimeProductCatalog.Gateway;
using RealTimeProductCatalog.Infrastructure.Configuration;
using RealTimeProductCatalog.Model.Entities;

namespace RealTimeProductCatalog.Consumer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private IApplicationSettings _applicationSettings { get; set; }
    private readonly ISinkGateway _sinkGateway;
    private readonly string _topic;

    public Worker(ILogger<Worker> logger, IApplicationSettings applicationSettings, ISinkGateway sinkGateway)
    {
        _logger = logger;
        _applicationSettings = applicationSettings;
        _topic = _applicationSettings.Kafka.Producer.Topic;
        _sinkGateway = sinkGateway;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _applicationSettings.Kafka.Cluster.Brokers,
            GroupId = _applicationSettings.Kafka.Consumer.GroupId,
            AutoOffsetReset = AutoOffsetReset.Latest,            
            EnableAutoCommit = _applicationSettings.Kafka.Consumer.EnableAutoCommit,
            EnableAutoOffsetStore = _applicationSettings.Kafka.Consumer.EnableAutoOffsetStore,
            AutoCommitIntervalMs = _applicationSettings.Kafka.Consumer.AutoCommitIntervalMs,           
        };        

        try
        {
            using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumerBuilder.Subscribe(_topic);
                var cancelToken = new CancellationTokenSource();
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        _logger.LogInformation("Consumer running at: {time}", DateTimeOffset.Now);
                        var consumeResult = consumerBuilder.Consume(cancelToken.Token);
                        if (consumeResult.IsPartitionEOF)
                        {
                            _logger.LogInformation($"Reached end of topic {consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");
                            continue;
                        }
                        if (consumeResult.Message is null)
                        {
                            _logger.LogInformation($"Message from {consumeResult.Topic}/{consumeResult.Partition} received from {consumeResult.Offset.Value} is null");
                            continue;
                        }

                        _logger.LogInformation($"Message from {consumeResult.Topic}/{consumeResult.Partition} received from {consumeResult.Offset.Value}");
                        var product = JsonSerializer.Deserialize<Product>(consumeResult.Message.Value);
                        if (product is null)
                        {
                            throw new ArgumentNullException(nameof(product));
                        }

                        var response = await _sinkGateway.Deliver(product);
                        if (!response.IsSuccessStatusCode)
                        {
                           consumerBuilder.Pause(new List<TopicPartition> { new TopicPartition(consumeResult.Topic, consumeResult.Partition) });
                        }else
                        {
                            consumerBuilder.Resume(new List<TopicPartition> { new TopicPartition(consumeResult.Topic, consumeResult.Partition)});
                        }                        
                    }
                }
                catch (OperationCanceledException e)
                {
                    _logger.LogInformation($"OperationCanceledException: {e.Message}");
                    consumerBuilder.Close();
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }
}
