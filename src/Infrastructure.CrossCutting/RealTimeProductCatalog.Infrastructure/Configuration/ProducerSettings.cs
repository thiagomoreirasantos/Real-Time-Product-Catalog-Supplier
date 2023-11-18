namespace RealTimeProductCatalog.Infrastructure.Configuration
{
    public class ProducerSettings
    {
        public required string Topic { get; set; }
        public required int Acks { get; set; }
    }
}