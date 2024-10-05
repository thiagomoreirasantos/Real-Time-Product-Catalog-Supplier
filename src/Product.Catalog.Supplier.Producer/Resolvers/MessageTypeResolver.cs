using KafkaFlow;

namespace Product.Catalog.Supplier.Producer.Resolvers
{
    public class MessageTypeResolver : IMessageTypeResolver
    {
        public Type OnConsume(IMessageContext context) => null;
        public void OnProduce(IMessageContext context)
        {
        }


    }
}
