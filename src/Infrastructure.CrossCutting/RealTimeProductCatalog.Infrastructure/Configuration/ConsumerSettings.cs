namespace RealTimeProductCatalog.Infrastructure.Configuration
{
    public class ConsumerSettings
    {        
        public required TopicSettings Topic { get; set; }
        public required int AutoCommitIntervalMs { get; set; }
        public required string GroupId { get; set; }
        public required string Workers { get; set; }
        public required string BufferSize { get; set; } 
        public required string EnsureMessageOrder { get; set; }
        public required string Enable { get; set; }
        public required bool EnableAutoCommit { get; set; }
        public required bool EnableAutoOffsetStore { get; set; }
    }
}