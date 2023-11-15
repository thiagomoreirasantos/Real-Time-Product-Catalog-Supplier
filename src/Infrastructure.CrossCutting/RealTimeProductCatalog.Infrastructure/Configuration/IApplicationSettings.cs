namespace RealTimeProductCatalog.Infrastructure.Configuration
{
    public interface IApplicationSettings
    {
        public KafkaSettings Kafka { get; set; }
    }
}