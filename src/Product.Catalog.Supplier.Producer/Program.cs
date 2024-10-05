using Microsoft.AspNetCore.Mvc;
using Product.Catalog.Supplier.Application.Configuration;
using Product.Catalog.Supplier.Application.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions<ApplicationSettings>().Bind(builder.Configuration.GetSection(nameof(ApplicationSettings))).ValidateDataAnnotations();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/v1/products/{stream_name}", ([FromRoute(Name = "stream_name")] string streamName, ProductData newProduct) =>
{
    ArgumentNullException.ThrowIfNull(newProduct);
})
.WithName("Products")
.WithOpenApi();

app.Run();