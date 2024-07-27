using System.Net;
using System.Net.Http.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Polly;
using Real_Time_Product_Catalog_Consumer.Configuration;

namespace Real_Time_Product_Catalog_Consumer;

public class Consumer : BackgroundService
{
    private readonly ILogger<Consumer> _logger;
    private readonly ConsumerConfig _kafkaConfiguration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AppSettings _appSettings;

    public Consumer(ILogger<Consumer> logger, IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _appSettings = appSettings.Value;
        _kafkaConfiguration = new ConsumerConfig
        {
            BootstrapServers = _appSettings.Brokers,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            Acks = Acks.All,
            GroupId = _appSettings.GroupId,
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string topic = _appSettings.Topic;
        bool isPause = false;
        int pauseCount = 0;

        using CancellationTokenSource cts = new();
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        using var consumer = new ConsumerBuilder<string, string>(_kafkaConfiguration).Build();

        consumer.Subscribe(topic);

        var pipelineBuilder = new ResiliencePipelineBuilder<HttpResponseMessage>()
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

        HttpClient httpClient = _httpClientFactory.CreateClient("url");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var cr = consumer.Consume(cts.Token);

                var pipeResponse = await pipelineBuilder.ExecuteAsync<HttpResponseMessage>(async token =>
                {
                    var response = await httpClient.PostAsJsonAsync("url", cr.Message, token);
                    if (!response.IsSuccessStatusCode && Volatile.Read(ref pauseCount) == 0)
                    {
                        consumer.Pause(consumer.Assignment);
                        isPause = true;
                        Interlocked.Increment(ref pauseCount);
                    }
                    return response;
                }, stoppingToken);

                if (isPause && pipeResponse.IsSuccessStatusCode)
                {
                    consumer.Resume(consumer.Assignment);
                }
            }
        }
        catch(Exception e)
        {
            _logger.LogError(e.ToString());
            throw;
        }
        finally
        {
            consumer.Close();
        }
    }
}
