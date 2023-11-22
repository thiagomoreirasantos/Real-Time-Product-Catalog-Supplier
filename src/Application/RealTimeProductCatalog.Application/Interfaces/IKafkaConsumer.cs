using Confluent.Kafka;

namespace RealTimeProductCatalog.Application.Interfaces
{
    public interface IKafkaConsumer
    {
        ConsumeResult<Ignore, string> ConsumeKafkaStream();
        void Pause();
        void Resume();
        void Subscribe(string topic);
    }
}