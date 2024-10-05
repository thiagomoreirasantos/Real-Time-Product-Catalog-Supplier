using KafkaFlow;
using KafkaFlow.Serializer;
using Product.Catalog.Supplier.Application.Configuration;
using Product.Catalog.Supplier.Producer.Resolvers;

namespace Product.Catalog.Supplier.Producer.Extensions
{
    public static class KafkaExtension
    {
        public static IServiceCollection AddKafkaProducer(this IServiceCollection services, WebApplicationBuilder builder)
        {
            //var applicationSettings = builder.Configuration.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>();

            //ArgumentNullException.ThrowIfNull(applicationSettings);

            //services.AddSingleton<IApplicationSettings>(applicationSettings);

            //services.AddKafka(kafka => kafka.AddCluster(config =>
            //{
            //    config.WithBrokers(brokers: applicationSettings.Kafka.Cluster.Brokers);
                
            //    foreach (KafkaProducerSettings producer1 in (IEnumerable<KafkaProducerSettings>)applicationSettings.Kafka.Producers)
            //    {
            //        KafkaProducerSettings producer = producer1;
            //        config.AddProducer(producer.Stream, (producerConfig =>
            //        {
            //            producerConfig.DefaultTopic(producer.Stream);
            //            producerConfig.WithProducerConfig(new Confluent.Kafka.ProducerConfig
            //            {
            //                MessageTimeoutMs = applicationSettings.Kafka.MessageTimeoutMs,
            //                SocketKeepaliveEnable = applicationSettings.Kafka.SocketKeepaliveEnable,
            //                ConnectionsMaxIdleMs = applicationSettings.Kafka.ConnectionsMaxIdleMs,
            //                MessageMaxBytes = applicationSettings.Kafka.MessageMaxBytes
            //            });
            //            producerConfig.AddMiddlewares(m => m.AddSerializer<NewtonsoftJsonSerializer, MessageTypeResolver>());
            //         }));                        
                    
            //    }
            //}));

            services.AddTransient<IMessageTypeResolver, MessageTypeResolver>();

            return services;

        }
    }
}
