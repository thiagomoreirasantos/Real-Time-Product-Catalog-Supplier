using RealTimeProductCatalog.Infrastructure.Interfaces;

namespace RealTimeProductCatalog.Infrastructure.Configuration
{
    public class ApplicationSettings: IApplicationSettings
    {
        public required KafkaSettings Kafka { get; set; }        
    }
}