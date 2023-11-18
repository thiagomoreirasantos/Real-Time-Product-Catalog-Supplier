using System.Text.Json;
using Confluent.Kafka;
using RealTimeProductCatalog.Infrastructure.Configuration;
using RealTimeProductCatalog.Model.Entities;

namespace RealTimeProductCatalog.Consumer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private IApplicationSettings _applicationSettings { get; set; }
    private readonly string _topic;

    public Worker(ILogger<Worker> logger, IApplicationSettings applicationSettings)
    {
        _logger = logger;
        _applicationSettings = applicationSettings;
        _topic = _applicationSettings.Kafka.Producer.Topic;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _applicationSettings.Kafka.Cluster.Brokers,
            GroupId = _applicationSettings.Kafka.Consumer.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,            
            EnableAutoCommit = true,
            EnableAutoOffsetStore = true,
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
                        _logger.LogInformation($"Message from {consumeResult.Topic}/{consumeResult.Partition} received from {consumeResult.Offset.Value}");
                        var product = JsonSerializer.Deserialize<Product>(consumeResult.Message.Value);
                        await Task.Delay(1000, stoppingToken);
                    }
                }
                catch (OperationCanceledException)
                {
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
