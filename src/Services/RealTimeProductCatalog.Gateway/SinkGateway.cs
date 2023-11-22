using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Polly.Retry;
using RealTimeProductCatalog.Application.Interfaces;
using RealTimeProductCatalog.Infrastructure.Configuration;
using RealTimeProductCatalog.Infrastructure.Interfaces;
using RealTimeProductCatalog.Model.Entities;

namespace RealTimeProductCatalog.Gateway
{
    public class SinkGateway : ISinkGateway
    {
        private readonly ILogger<SinkGateway> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IApplicationSettings _appssettings;
        private readonly IRetryPolicyHandler _retryPolicyHandler;

        public SinkGateway(ILogger<SinkGateway> logger, IHttpClientFactory httpClientFactory, IApplicationSettings appssettings, IRetryPolicyHandler retryPolicyHandler)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _appssettings = appssettings;
            _retryPolicyHandler = retryPolicyHandler;
        }

        public async Task<HttpResponseMessage> Deliver(Product product)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(_appssettings.Kafka.Destination.Name);


                var httpRequestMessage = new HttpRequestMessage(new HttpMethod(_appssettings.Kafka.Destination.Method), _appssettings.Kafka.Destination.Url)
                {
                    Content = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json"),
                };                

                var response = await _retryPolicyHandler.GetPolicy().ExecuteAsync(async () =>  await client.SendAsync(httpRequestMessage));

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Message delivered to {response?.RequestMessage?.RequestUri}");
                }
                else
                {
                    _logger.LogError($"Failed to deliver message to {response?.RequestMessage?.RequestUri}");
                }

                return response ?? new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Failed to deliver message to sink")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to deliver message to sink");

                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message)
                };
            }
        }
    }
}