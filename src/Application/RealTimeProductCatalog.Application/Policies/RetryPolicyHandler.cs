using System.Net;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace RealTimeProductCatalog.Application.Policies
{
    public class RetryPolicyHandler : IRetryPolicyHandler
    {
        private readonly ILogger<RetryPolicyHandler> _logger;             

        public RetryPolicyHandler(ILogger<RetryPolicyHandler> logger)
        {
            _logger = logger;            
        }

        public IAsyncPolicy<HttpResponseMessage> GetPolicy()
        {
            int jitter = new Random().Next(0, 1000);

            return Policy
                .HandleResult<HttpResponseMessage>(res => res.StatusCode == HttpStatusCode.InternalServerError)
                .WaitAndRetryAsync(Backoff.ExponentialBackoff(TimeSpan.FromSeconds(2) + TimeSpan.FromMilliseconds(jitter), retryCount: 5, 2),
                onRetry: (response, timeSpan, retryCount, context) =>
                {
                    _logger.LogError($"Failed to deliver message to sink. Retrying {retryCount}.");
                });
        }
    }
}