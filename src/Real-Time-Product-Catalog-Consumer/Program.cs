using Real_Time_Product_Catalog_Consumer;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<MessageConsumer>();

var services = new ServiceCollection();
services.AddMassTransit(x =>
{
    x.UsingInMemory();

    x.AddRider(rider =>
    {   
        rider.AddConsumer<MessageConsumer>();

        rider.UsingKafka((context, k) =>
        {
            k.Host("localhost:9092,localhost:9093,localhost:9094");

            k.TopicEndpoint<Message>("produtct-catalog","productsink",e=>
            {
                e.ConfigureConsumer<MessageConsumer>(context);
            });
        });
    });
});

var host = builder.Build();
host.Run();