using MassTransit;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace Real_Time_Product_Catalog_Consumer.Services;

public class Dispatcher(IHttpClientFactory _httpClientFactory): IDispatcher
{
    public async Task SendMessageAsync(ConsumeContext<Message> consumeContext)
    {
        using CancellationTokenSource cancellationTokenSource = new();
        var CancellationToken = cancellationTokenSource.Token;

        HttpClient httpClient = _httpClientFactory.CreateClient("url");        

        var pipeline = RetryManager.AddRetry();

        var response = await pipeline.ExecuteAsync<HttpResponseMessage>(async token => 
        {
            return await httpClient.PostAsJsonAsync("url","", token);            
        }, cancellationToken: CancellationToken);        
    }
}