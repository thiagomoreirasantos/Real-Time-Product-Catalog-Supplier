using System.Text.Json;
using RealTimeProductCatalog.Infrastructure.Configuration;
using RealTimeProductCatalog.Model.Entities;
using RealTimeProductCatalog.Producer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var appsettings = builder.Configuration.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>() ?? throw new InvalidOperationException("Unable to get appsettings");
builder.Services.AddSingleton<IApplicationSettings>(appsettings);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "RealTimeProductCatalog.Api", Version = "v1" });
});

builder.Services.AddHttpClient();
builder.Services.AddHttpClient("kafka", c =>
{
    c.BaseAddress = new Uri(appsettings.Kafka.Cluster.Brokers);
    c.Timeout = TimeSpan.FromSeconds(30);
}).SetHandlerLifetime(TimeSpan.FromMinutes(5));

builder.Services.AddScoped<IPublisher, Publisher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapPost("/api/v1/products", async (IPublisher producer, IApplicationSettings applicationSettings, Product product) =>
{
    var result = await producer.StartSendingMessages(JsonSerializer.Serialize(product));
    if (!result)
    {
        return Results.BadRequest();
    }    
    return Results.Ok();
});

app.Run();
