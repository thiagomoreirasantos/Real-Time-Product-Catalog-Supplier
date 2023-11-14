namespace RealTimeProductCatalog.Infrastructure.Configuration
{
    public class TopicSettings
    {
        public required string Name { get; set; }
        public required string Destination { get; set; }
        public required string Method { get; set; }
    }
}