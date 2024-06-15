using MassTransit;
using MassTransit.KafkaIntegration;

namespace Real_Time_Product_Catalog_Consumer.Services;

public interface IDispatcher
{
    Task SendMessageAsync(ConsumeContext<Message> consumeContext);
}