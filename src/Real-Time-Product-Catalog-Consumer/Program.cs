using Real_Time_Product_Catalog_Consumer;
using Real_Time_Product_Catalog_Consumer.Configuration;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        var env = context.HostingEnvironment;

        config.SetBasePath(Directory.GetCurrentDirectory());
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
        config.AddEnvironmentVariables();

        if (args != null)
        {
            config.AddCommandLine(args);
        }
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<AppSettings>(hostContext.Configuration.GetSection("AppSettings"));
        services.PostConfigure<AppSettings>(options =>
        {
            services.AddHttpClient(options.Url, config =>
            {
                config.BaseAddress = new Uri(options.Url);
                config.Timeout = TimeSpan.FromMinutes(5);
            });
        });
        services.AddHostedService<Consumer>();
    });

var host = builder.Build();
await host.RunAsync();