using RealTimeProductCatalog.Model.Entities;

namespace RealTimeProductCatalog.Gateway
{
    public interface ISinkGateway
    {
        Task<HttpResponseMessage> Deliver(Product product);
    }
}