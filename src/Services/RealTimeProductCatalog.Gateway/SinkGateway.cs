using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RealTimeProductCatalog.Infrastructure.Configuration;
using RealTimeProductCatalog.Model.Entities;

namespace RealTimeProductCatalog.Gateway
{
    public class SinkGateway : ISinkGateway
    {
        private readonly ILogger<SinkGateway> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IApplicationSettings _appssettings;

        public SinkGateway(ILogger<SinkGateway> logger, IHttpClientFactory httpClientFactory, IApplicationSettings appssettings)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _appssettings = appssettings;            
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

                var response = await client.SendAsync(httpRequestMessage);

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