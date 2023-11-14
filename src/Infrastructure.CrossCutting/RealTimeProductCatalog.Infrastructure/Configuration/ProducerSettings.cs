namespace RealTimeProductCatalog.Infrastructure.Configuration
{
    public class ProducerSettings
    {
        public required string Topics { get; set; }
        public required string Acks { get; set; }
    }
}