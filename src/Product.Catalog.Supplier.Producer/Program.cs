using Microsoft.AspNetCore.Mvc;
using Product.Catalog.Supplier.Application.Configuration;
using Product.Catalog.Supplier.Application.Entities;
using Product.Catalog.Supplier.Application.Services;
using Product.Catalog.Supplier.DataContracts;
using Product.Catalog.Supplier.Producer.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions<ApplicationSettings>().Bind(builder.Configuration.GetSection(nameof(ApplicationSettings))).ValidateDataAnnotations();
builder.Services.AddKafkaProducer(builder);
builder.Services.AddTransient<IStreamService, StreamService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/v1/products/{stream_name}", ([FromRoute(Name = "stream_name")] string streamName, HttpContext context, ProductData newProduct, IStreamService streamService) =>
{
    if (string.IsNullOrEmpty(streamName)) Results.BadRequest();

    if (newProduct == null) return Results.BadRequest();

    if (streamService is null) return Results.BadRequest();

    var messageDataHeaders = new MessageDataHeaders
    {
        MessageId = context.Request.Headers["message_id"].ToString(),
        CorrelationId = context.Request.Headers["correlation_id"].ToString(),
        PartitionKey = context.Request.Headers["partition_key"].FirstOrDefault(),
        MessageType = context.Request.Headers["message_type"].ToString(),
        Timestamp = context.Request.Headers["timestamp"].ToString()
    };

    var product = new ProductDto(streamName,
        messageDataHeaders.MessageId ?? string.Empty,
        messageDataHeaders.CorrelationId ?? string.Empty,
        messageDataHeaders.PartitionKey ?? string.Empty,
        messageDataHeaders.MessageType ?? string.Empty,
        messageDataHeaders.Timestamp ?? string.Empty,
        Payload: newProduct);

    var streamResult = streamService?.HandleMessage(product);
    if (streamResult is null) return Results.BadRequest();

    return Results.Ok();
    
})
.Produces<ProductData>(StatusCodes.Status201Created) 
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status500InternalServerError)
.WithName("Products")
.WithOpenApi();

app.Run();