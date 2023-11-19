using System.Text.Json;
using RealTimeProductCatalog.Application.Policies;
using RealTimeProductCatalog.Application.Service;
using RealTimeProductCatalog.Application.Validation;
using RealTimeProductCatalog.Infrastructure.Configuration;
using RealTimeProductCatalog.Model.Entities;
using RealTimeProductCatalog.Producer;

var builder = WebApplication.CreateBuilder(args);

#region [Add services to the container.]
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#endregion

#region [Swagger]
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "RealTimeProductCatalog.Api", Version = "v1" });
});
#endregion

#region [Dependency Injection]
var appsettings = builder.Configuration.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>() ?? throw new InvalidOperationException("Unable to get appsettings");
builder.Services.AddSingleton<IApplicationSettings>(appsettings);

builder.Services.AddScoped<IPublisher, Publisher>();
builder.Services.AddScoped<IRetryPolicyHandler, RetryPolicyHandler>();
builder.Services.AddScoped<IProductHandler, ProductHandler>();
#endregion

#region [HttpClient]
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("kafka", c =>
{
    c.BaseAddress = new Uri(appsettings.Kafka.Cluster.Brokers);
    c.Timeout = TimeSpan.FromSeconds(30);
}).SetHandlerLifetime(TimeSpan.FromMinutes(5));
#endregion

var app = builder.Build();

#region [Configure the HTTP request pipeline.]
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#endregion

#region [Endpoints]

app.MapPost("/api/v1/products", async (IPublisher producer, IApplicationSettings applicationSettings, Product product) =>
{
    var validator = new ProductValidator();
    var validationResult = await validator.ValidateAsync(product);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors);
    }
    
    var result = await producer.StartSendingMessages(JsonSerializer.Serialize(product));
    if (!result)
    {
        return Results.StatusCode(500);
    }    
    return Results.Created("/api/v1/products",result);
});

#endregion

app.Run();