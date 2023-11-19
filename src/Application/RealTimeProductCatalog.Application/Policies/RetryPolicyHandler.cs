using System.Net;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Contrib.WaitAndRetry;
using RealTimeProductCatalog.Infrastructure.Configuration;

namespace RealTimeProductCatalog.Application.Policies
{
    public class RetryPolicyHandler : IRetryPolicyHandler
    {
        private readonly ILogger<RetryPolicyHandler> _logger;             
        private readonly IApplicationSettings _applicationSettings;

        public RetryPolicyHandler(ILogger<RetryPolicyHandler> logger, IApplicationSettings applicationSettings)
        {
            _logger = logger;
            _applicationSettings = applicationSettings;
        }

        public IAsyncPolicy<HttpResponseMessage> GetPolicy()
        {
            HttpStatusCode[] httpStatusCodesWorthRetrying = _applicationSettings.Kafka.Destination.RetryCodes ?? new HttpStatusCode[] { HttpStatusCode.InternalServerError };
            int jitter = new Random().Next(0, 1000);

            return Policy
                .HandleResult<HttpResponseMessage>(res => httpStatusCodesWorthRetrying.Contains(res.StatusCode))
                .WaitAndRetryAsync(Backoff.ExponentialBackoff(TimeSpan.FromSeconds(2) + TimeSpan.FromMilliseconds(jitter), retryCount: 5, 2),
                onRetry: (response, timeSpan, retryCount, context) =>
                {
                    _logger.LogError($"Failed to deliver message to sink. Retrying {retryCount}.");
                });
        }
    }
}