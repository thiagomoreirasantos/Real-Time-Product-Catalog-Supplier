using System.Net;
using Confluent.Kafka;
using RealTimeProductCatalog.Infrastructure.Configuration;

namespace RealTimeProductCatalog.Producer
{
    public class Publisher
    {
        private readonly IApplicationSettings _appsettings;
        private readonly ProducerConfig _producerConfig;
        public Publisher(IApplicationSettings appsettings)
        {
            _appsettings = appsettings;

            _producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
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
            
            var deliveryReport = await producer.ProduceAsync("", new Message<long, string>
            {
                Key = DateTime.UtcNow.Ticks,
                Value = content
            });
        }
    }
}
