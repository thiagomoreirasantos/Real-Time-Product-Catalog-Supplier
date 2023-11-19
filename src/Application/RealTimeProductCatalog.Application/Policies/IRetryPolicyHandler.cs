using Polly;

namespace RealTimeProductCatalog.Application.Policies
{
    public interface IRetryPolicyHandler
    {
        IAsyncPolicy<HttpResponseMessage> GetPolicy();
    }
}