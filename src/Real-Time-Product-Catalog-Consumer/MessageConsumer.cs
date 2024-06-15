using MassTransit;
using MassTransit.KafkaIntegration;
using Real_Time_Product_Catalog_Consumer.Services;
using System.Net.Http;
using System.Net.Http.Json;

namespace Real_Time_Product_Catalog_Consumer;

public class MessageConsumer: IConsumer<Message>
{    
    
    public Task Consume(ConsumeContext<Message> context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        return Task.CompletedTask;     
    }
}