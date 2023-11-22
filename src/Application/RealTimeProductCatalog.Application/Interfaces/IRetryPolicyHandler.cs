using Polly;

namespace RealTimeProductCatalog.Application.Interfaces
{
    public interface IRetryPolicyHandler
    {
        IAsyncPolicy<HttpResponseMessage> GetPolicy();
    }
}