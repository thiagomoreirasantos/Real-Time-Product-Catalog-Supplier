var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(( hostBuilderContext, services) =>
    {        
        var appsettings = hostBuilderContext.Configuration.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>() ?? throw new InvalidOperationException("Unable to get appsettings");

        services.AddSingleton<IApplicationSettings>(appsettings);

        services.AddHostedService<Worker>();
    });

var host = builder.Build();
await host.RunAsync();
 