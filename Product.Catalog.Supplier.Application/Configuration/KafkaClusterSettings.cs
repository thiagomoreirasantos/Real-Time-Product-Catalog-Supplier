namespace Product.Catalog.Supplier.Application.Configuration
{
    public class KafkaClusterSettings
    {
        public required IList<string> Brokers { get; set; }
    }
}
