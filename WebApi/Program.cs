using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;


// https://github.com/Azure/azure-functions-core-tools/issues/2413#issuecomment-1260415071
var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((appBuilder, services) =>
    {
        // Use Serilog for logging.
        // Serilog is configured via Env Vars .. which is
        //   => Localhost: local.settings.json
        //   => Azure Functions on Azure: the 'Configuration' tab/section/page
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(appBuilder.Configuration) // <-- This is the magic, here!
            .Enrich.FromLogContext()
            .CreateLogger();
        services.AddLogging(configure => configure.AddSerilog(logger, true));

        // Setup our configuration settings.
        // Just a strongly typed class.
        //services
        //    //.AddOptions<ConnectionStringOptions>()
        //    .Configure<IConfiguration>((settings, configuration) =>
        //    {
        //        configuration
        //            .GetSection(ConnectionStringOptions.ConfigurationSectionKey)
        //            .Bind(settings);
        //    });

        // DapperRepository requires an instance of an IOptions<ConnectionStringOptions> class
        // in the ctor .. which is why that is registered, above.
        //services.AddSingleton<IRepository, DapperRepository>();
    })
    .Build();

host.Run();