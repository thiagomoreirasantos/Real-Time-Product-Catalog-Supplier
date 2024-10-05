namespace Product.Catalog.Supplier.Application.Configuration
{
    public class KafkaProducerSettings
    {
        public required string Stream { get; set; }

        public required string Acks { get; set; }

        public bool Gzip { get; set; }
    }
}
