using Confluent.Kafka;
using RealTimeProductCatalog.Infrastructure.Configuration;

namespace RealTimeProductCatalog.Consumer
{
    public class Consumer : IConsumer
    {
        public readonly IApplicationSettings _applicationSettings;
        private readonly ConsumerConfig _consumerConfig;
        private readonly ILogger<Consumer> _logger;

        public Consumer(IApplicationSettings applicationSettings, ILogger<Consumer> logger)
        {
            _applicationSettings = applicationSettings;
            _logger = logger;

            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _applicationSettings.Kafka.Cluster.Brokers,
                GroupId = _applicationSettings.Kafka.Consumer.GroupId,
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = _applicationSettings.Kafka.Consumer.EnableAutoCommit,
                EnableAutoOffsetStore = _applicationSettings.Kafka.Consumer.EnableAutoOffsetStore,
                AutoCommitIntervalMs = _applicationSettings.Kafka.Consumer.AutoCommitIntervalMs,
            };
        }

        public ConsumeResult<Ignore, string> ConsumeKafkaStream()
        {
            var cancelToken = new CancellationTokenSource();

            ConsumeResult<Ignore, string> consumeResult = new ConsumeResult<Ignore, string>();

            try
            {
                using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
                {
                    try
                    {
                        consumer.Subscribe(_applicationSettings.Kafka.Consumer.Topic.Name);

                        consumeResult = consumer.Consume(cancelToken.Token);

                        _logger.LogInformation($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");

                        return consumeResult;

                    }
                    catch (OperationCanceledException e)
                    {
                        _logger.LogInformation($"OperationCanceledException: {e.Message}");
                        consumer.Close();
                    }

                    return consumeResult;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ConsumeResult<Ignore, string>();
            }
        }
        
        public void Pause(List<TopicPartition> topicPartitions)
        {
            var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
            consumer.Pause(topicPartitions);
        }

        public void Resume(List<TopicPartition> topicPartitions)
        {
            var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
            consumer.Resume(topicPartitions);
        }
    }
}