using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;


var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(builder =>
    {
        builder.AddConfiguration(GetConfigurationBuilder());
    })
    .ConfigureServices((appBuilder, services) =>
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(appBuilder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();
        services.AddLogging(configure => configure.AddSerilog(logger, true));

        // Setup our configuration settings.
        //services
        //    .AddOptions<ConnectionStringOptions>()
        //    .Configure<IConfiguration>((settings, configuration) =>
        //    {
        //        configuration
        //            .GetSection(ConnectionStringOptions.ConfigurationSectionKey)
        //            .Bind(settings);
        //    });

        //services.AddSingleton<IRepository, DapperRepository>();
    })
    .Build();


host.Run();


// Strongly based off/from: https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.host.createdefaultbuilder?view=dotnet-plat-ext-5.0
static IConfiguration GetConfigurationBuilder() =>
    new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddUserSecrets<Program>()
        .AddEnvironmentVariables()
        .Build();