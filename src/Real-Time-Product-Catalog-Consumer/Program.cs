using Real_Time_Product_Catalog_Consumer;
using MassTransit;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<PoolingService>();

var services = new ServiceCollection();

services.AddHttpClient("",config =>
{
    config.BaseAddress = new Uri("");
    config.Timeout = TimeSpan.FromMinutes(5);
});

services.AddMassTransit(x =>
{
    x.UsingInMemory();

    x.AddRider(rider =>
    {   
        rider.AddConsumer<MessageConsumer>();

        rider.UsingKafka((context, cfg) =>
        {
            cfg.Host("localhost:9092,localhost:9093,localhost:9094");

            cfg.TopicEndpoint<Message>("produtct-catalog","productsink",e=>
            {
                e.ConfigureConsumer<MessageConsumer>(context);                
            });
        });
    });
});

var host = builder.Build();
host.Run();