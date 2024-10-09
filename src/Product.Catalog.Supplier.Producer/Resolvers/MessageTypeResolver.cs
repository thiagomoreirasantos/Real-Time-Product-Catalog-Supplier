using KafkaFlow;

namespace Product.Catalog.Supplier.Producer.Resolvers
{
    public class MessageTypeResolver : IMessageTypeResolver
    {
        public Type? OnConsume(IMessageContext context)
        {
            return null;
        }

        public void OnProduce(IMessageContext context)
        {
        }


    }
}
