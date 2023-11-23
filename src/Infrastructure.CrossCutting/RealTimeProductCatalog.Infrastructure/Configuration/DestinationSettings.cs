namespace RealTimeProductCatalog.Infrastructure.Configuration
{
    public class DestinationSettings
    {
        public required string Name { get; set; }
        public required string Url { get; set; }
        public required string Method { get; set; }
        public int RetryCount { get; set; }
        public int Base { get; set; }
        public required HttpStatusCode[] RetryCodes { get; set; }
    }
}