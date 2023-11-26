namespace RealTimeProductCatalog.Application.Interfaces
{
    public interface ISinkGateway
    {
        Task<HttpResponseMessage> Deliver(Product product);
    }
}