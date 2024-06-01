using MassTransit;
using System.Net;
using System.Net.Http;

namespace Real_Time_Product_Catalog_Consumer.Services.Gateways;

public class Dispatcher(IHttpClientFactory _httpClientFactory)
{

    public async Task SendMessage(ConsumeContext<Message> consumeContext)
    {
        using CancellationTokenSource cancellationTokenSource = new();
        var CancellationToken = cancellationTokenSource.Token;
        HttpClient httpClient = _httpClientFactory.CreateClient("");

        using HttpRequestMessage request = new(new HttpMethod("POST"),"")
        {
            
        };

        await httpClient.SendAsync(request,CancellationToken);
    }
}