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
builder.Services.AddHttpClient(appsettings.Kafka.Destination.Name, c =>
{
    c.BaseAddress = new Uri(appsettings.Kafka.Destination.Url);
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

app.MapPost("/api/v1/products", async (IProductHandler handler, IApplicationSettings applicationSettings, ProductInput product) =>
{
    var productEntity = ProductMap.Map(product);
    var validationResult = await handler.Handle(productEntity);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors);
    }
    
    var result = await handler.PublishMessage(productEntity);
    if (!result)
    {
        return Results.StatusCode(500);
    }    
    return Results.Created("/api/v1/products",result);
});

#endregion

app.Run();