using System.Net;
using Confluent.Kafka;
using RealTimeProductCatalog.Infrastructure.Configuration;

namespace RealTimeProductCatalog.Producer
{
    public class Publisher: IPublisher
    {
        private readonly IApplicationSettings _appsettings;
        private readonly ProducerConfig _producerConfig;
        private readonly string _topic;

        public Publisher(IApplicationSettings appsettings)
        {
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

        public async Task StartSendingMessages(string content)
        {
            using var producer = new ProducerBuilder<long, string>(_producerConfig)
                .SetKeySerializer(Serializers.Int64)
                .SetValueSerializer(Serializers.Utf8)
                .SetLogHandler((_, message) =>
                    Console.WriteLine($"Facility: {message.Facility}-{message.Level} Message: {message.Message}"))
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}. Is Fatal: {e.IsFatal}"))
                .Build();
            
            var deliveryReport = await producer.ProduceAsync(_topic, new Message<long, string>
            {
                Key = DateTime.UtcNow.Ticks,
                Value = content
            });
        }
    }
}
