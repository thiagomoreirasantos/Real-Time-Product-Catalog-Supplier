using System.Net;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using RealTimeProductCatalog.Application.Interfaces;
using RealTimeProductCatalog.Infrastructure.Configuration;
using RealTimeProductCatalog.Infrastructure.Interfaces;

namespace RealTimeProductCatalog.Producer
{
    public class Publisher : IPublisher
    {
        private readonly IApplicationSettings _appsettings;
        private readonly ProducerConfig _producerConfig;
        private readonly ILogger<Publisher> _logger;

        private readonly string _topic;

        public Publisher(IApplicationSettings appsettings)
        {
            _logger = new LoggerFactory().CreateLogger<Publisher>();
            _logger.LogInformation("Publisher created");

            _appsettings = appsettings;

            _topic = _appsettings.Kafka.Producer.Topic;

            _producerConfig = new ProducerConfig
            {
                BootstrapServers = _appsettings.Kafka.Cluster.Brokers,
                EnableDeliveryReports = true,
                ClientId = Dns.GetHostName(),
                Acks = Acks.All,
                MessageSendMaxRetries = 3,
                RetryBackoffMs = 1000,
                EnableIdempotence = true
            };
        }

        public async Task<bool> StartSendingMessages(string content)
        {
            using var producer = new ProducerBuilder<long, string>(_producerConfig)
                .SetKeySerializer(Serializers.Int64)
                .SetValueSerializer(Serializers.Utf8)
                .SetLogHandler((_, message) =>
                    _logger.LogInformation($"Facility: {message.Facility}-{message.Level} Message: {message.Message}"))
                .SetErrorHandler((_, e) => _logger.LogError($"Error: {e.Reason}. Is Fatal: {e.IsFatal}"))
                .Build();

            var deliveryReport = await producer.ProduceAsync(_topic, new Message<long, string>
            {
                Key = DateTime.UtcNow.Ticks,
                Value = content
            });

            if (deliveryReport.Status == PersistenceStatus.PossiblyPersisted)
            {
                _logger.LogInformation($"Message produced to {deliveryReport.Topic}/{deliveryReport.Partition} with offset: {deliveryReport.Offset.Value}");
                return true;
            }

            _logger.LogError($"Failed to produce message to {deliveryReport.Topic}/{deliveryReport.Partition} with offset: {deliveryReport.Offset.Value}");
            return false;
        }
    }
}
