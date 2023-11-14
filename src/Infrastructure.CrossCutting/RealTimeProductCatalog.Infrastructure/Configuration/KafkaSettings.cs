namespace RealTimeProductCatalog.Infrastructure.Configuration
{
    public class KafkaSettings
    {
        public required IList<ConsumerSettings> Consumers { get; set; }
        public required IList<ClusterSettings> Cluster { get; set; }
        public required IList<ProducerSettings> Producers { get; set; }
    }
}