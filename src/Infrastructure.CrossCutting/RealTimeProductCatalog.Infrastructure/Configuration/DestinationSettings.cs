namespace RealTimeProductCatalog.Infrastructure.Configuration
{
    public class DestinationSettings
    {
        public required string Name { get; set; }
        public required string Url { get; set; }
        public required string Method { get; set; }
    }
}