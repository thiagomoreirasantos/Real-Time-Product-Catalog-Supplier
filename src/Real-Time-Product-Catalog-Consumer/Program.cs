using Real_Time_Product_Catalog_Consumer;
using Real_Time_Product_Catalog_Consumer.Configuration;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Consumer>();

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.PostConfigure<AppSettings>(options =>
{
    builder.Services.AddHttpClient(options.Url, config =>
    {
        config.BaseAddress = new Uri(options.Url);
        config.Timeout = TimeSpan.FromMinutes(5);
    });
});

var host = builder.Build();
await host.RunAsync();