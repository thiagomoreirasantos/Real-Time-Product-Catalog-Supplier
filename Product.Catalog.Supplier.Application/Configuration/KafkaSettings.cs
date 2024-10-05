namespace Product.Catalog.Supplier.Application.Configuration
{
    public class KafkaSettings
    {
        public KafkaClusterSettings Cluster { get; set; }

        public int MessageTimeoutMs { get; set; }

        public bool SocketKeepaliveEnable { get; set; }

        public int ConnectionsMaxIdleMs { get; set; }

        public int MessageMaxBytes { get; set; }

        public required IList<KafkaProducerSettings> Producers { get; set; }
    }
}
