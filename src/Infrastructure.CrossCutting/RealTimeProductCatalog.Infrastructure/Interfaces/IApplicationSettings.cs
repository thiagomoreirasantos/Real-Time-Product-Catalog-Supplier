using RealTimeProductCatalog.Infrastructure.Configuration;

namespace RealTimeProductCatalog.Infrastructure.Interfaces
{
    public interface IApplicationSettings
    {
        public KafkaSettings Kafka { get; set; }
    }
}