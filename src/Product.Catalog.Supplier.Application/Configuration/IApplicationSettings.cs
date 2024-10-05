namespace Product.Catalog.Supplier.Application.Configuration
{
    public interface IApplicationSettings
    {
        KafkaSettings Kafka { get; }
    }
}
