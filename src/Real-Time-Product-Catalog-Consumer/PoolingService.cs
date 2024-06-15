using MassTransit;
using MassTransit.KafkaIntegration;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Real_Time_Product_Catalog_Consumer;

public class PoolingService : BackgroundService
{
    private readonly ILogger<PoolingService> _logger;
    private readonly IBusControl _busControl;

    public PoolingService(ILogger<PoolingService> logger, IBusControl busControl)
    {
        _logger = logger;        
        _busControl = busControl;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {        
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Consumer running at: {time}", DateTimeOffset.Now);
            }

            var handle = _busControl.ConnectReceiveEndpoint("topic", endpointConfigurator =>
            {
                endpointConfigurator.Consumer<MessageConsumer>();

                endpointConfigurator.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));

                //endpointConfigurator.UseInMemoryOutbox();
            });

            await Task.Delay(1000, stoppingToken);
        }
    }
}
