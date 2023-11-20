using System.Text.Json;
using RealTimeProductCatalog.Gateway;
using RealTimeProductCatalog.Infrastructure.Configuration;
using RealTimeProductCatalog.Model.Entities;

namespace RealTimeProductCatalog.Consumer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private IApplicationSettings _applicationSettings { get; set; }
    private readonly ISinkGateway _sinkGateway;
    private readonly IConsumer _consumer;
    private readonly string _topic;

    public Worker(ILogger<Worker> logger, IApplicationSettings applicationSettings, ISinkGateway sinkGateway, IConsumer consumer)
    {
        _logger = logger;
        _applicationSettings = applicationSettings;
        _topic = _applicationSettings.Kafka.Producer.Topic;
        _sinkGateway = sinkGateway;
        _consumer = consumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Consumer running at: {time}", DateTimeOffset.Now);

                var consumeResult =_consumer.ConsumeKafkaStream();

                var product = JsonSerializer.Deserialize<Product>(consumeResult.Message.Value);
                if (product is null)
                {
                    throw new ArgumentNullException(nameof(product));
                }
                var response = await _sinkGateway.Deliver(product);
            }
        }
        catch (Exception e)
        {
            _logger.LogInformation($"OperationCanceledException: {e.Message}");
        }
    }
}

