using System.Net;
using Polly;

namespace Real_Time_Product_Catalog_Consumer.Services;

public static class RetryManager
{
    public static ResiliencePipeline<HttpResponseMessage> AddRetry()
    {
        return new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddRetry(new Polly.Retry.RetryStrategyOptions<HttpResponseMessage>()
            {
                DelayGenerator = args =>
                {
                    var delay = args.AttemptNumber switch
                    {
                        >= 0 => TimeSpan.FromSeconds(Math.Pow(2, 3)),
                        _ => TimeSpan.Zero
                    };

                    return new ValueTask<TimeSpan?>(delay);
                },

                MaxRetryAttempts = 3,
                UseJitter = true,
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
        .HandleResult(response => response.StatusCode == HttpStatusCode.InternalServerError),
                Delay = TimeSpan.FromSeconds(1),
                OnRetry = retryArguments =>
                {
                    
                    return ValueTask.CompletedTask;
                }
            }).Build();
    }
}