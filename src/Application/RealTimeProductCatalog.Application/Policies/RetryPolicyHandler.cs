namespace RealTimeProductCatalog.Application.Policies
{
    public class RetryPolicyHandler : IRetryPolicyHandler
    {
        private readonly ILogger<RetryPolicyHandler> _logger;
        private readonly IApplicationSettings _applicationSettings;
        private readonly IKafkaConsumer _consumer;
        private readonly object _lockPause = new();
        private readonly object _lockResume = new();

        public RetryPolicyHandler(ILogger<RetryPolicyHandler> logger, IApplicationSettings applicationSettings, IKafkaConsumer consumer)
        {
            _logger = logger;
            _applicationSettings = applicationSettings;
            _consumer = consumer;
        }

        public IAsyncPolicy<HttpResponseMessage> GetPolicy()
        {
            HttpStatusCode[] httpStatusCodesWorthRetrying = _applicationSettings.Kafka.Destination.RetryCodes ?? new HttpStatusCode[] { HttpStatusCode.InternalServerError };
            int jitter = new Random().Next(0, 1000);

            AsyncRetryPolicy<HttpResponseMessage> asyncRetryPolicy = Policy
                .HandleResult<HttpResponseMessage>(res => httpStatusCodesWorthRetrying.Contains(res.StatusCode))
                .WaitAndRetryAsync(Backoff.ExponentialBackoff(TimeSpan.FromSeconds(2) + TimeSpan.FromMilliseconds(jitter), retryCount: 5, 2),
                onRetry: (response, timeSpan, retryCount, context) =>
                {
                    _logger.LogError($"Failed to deliver message to sink. Retrying {retryCount}.");

                    if (retryCount == 1)
                    {
                        lock (_lockPause)
                        {
                            context["PolicyKey"] = Guid.NewGuid().ToString();
                            _consumer.Pause();
                        }
                    }
                });

            AsyncFallbackPolicy<HttpResponseMessage> fallbackPolicy = Policy
                .HandleResult<HttpResponseMessage>(res => httpStatusCodesWorthRetrying.Contains(res.StatusCode))
                .OrResult(res => res.IsSuccessStatusCode)
                .FallbackAsync(async (res, context, token) =>
                {
                    _logger.LogError($"Failed to deliver message to sink. Retrying {context.PolicyKey}.");

                    lock (_lockResume)
                    {
                        if (context.PolicyKey == Guid.NewGuid().ToString())
                        {
                            _consumer.Resume();
                        }
                    }

                    return await Task.FromResult(res.Result).ConfigureAwait(false);

                }, onFallbackAsync: (delegateResult, context) =>
                {
                    _logger.LogError($"Fallback executed for {context.PolicyKey}.");
                    return Task.CompletedTask;
                });

            return asyncRetryPolicy.WrapAsync(fallbackPolicy);
        }
    }
}