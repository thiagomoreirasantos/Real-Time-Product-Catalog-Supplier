namespace RealTimeProductCatalog.Infrastructure.Configuration
{
    public class ConsumerSettings
    {        
        public required IList<TopicSettings> Topics { get; set; }
        public required string AutoCommitIntervalMs { get; set; }
        public required string GroupId { get; set; }
        public required string Workers { get; set; }
        public required string BufferSize { get; set; } 
        public required string EnsureMessageOrder { get; set; }
        public required string Enable { get; set; }
    }
}