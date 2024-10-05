namespace Product.Catalog.Supplier.Application.Configuration
{
    public class ApplicationSettings: IApplicationSettings
    {
        public required KafkaSettings Kafka { get; set; }
        public long MaxRequestBodySize { get; set; }
    }
}
