namespace Product.Catalog.Supplier.Application.Configuration
{
    public class ApplicationSettings
    {
        public KafkaSettings Kafka { get; set; }
        public long MaxRequestBodySize { get; set; }
    }
}
