namespace RealTimeProductCatalog.Infrastructure.Configuration
{
    public class ClusterSettings
    {
        public required IList<string> Brokers { get; set; }
    }
}