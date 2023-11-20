using Confluent.Kafka;

namespace RealTimeProductCatalog.Consumer
{
    public interface IConsumer
    {
        ConsumeResult<Ignore, string> ConsumeKafkaStream();
        void Pause(List<TopicPartition> topicPartitions);
        void Resume(List<TopicPartition> topicPartitions);
    }
}