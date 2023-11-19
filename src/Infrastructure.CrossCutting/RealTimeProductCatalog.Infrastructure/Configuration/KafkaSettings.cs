namespace RealTimeProductCatalog.Infrastructure.Configuration
{
    public class KafkaSettings
    {
        public required ConsumerSettings Consumer { get; set; }
        public required ClusterSettings Cluster { get; set; }
        public required ProducerSettings Producer { get; set; }
        public required DestinationSettings Destination { get; set; }
    }
}