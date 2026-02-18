using TeamsTranscriptBot.Models;
using TeamsTranscriptBot.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((ctx, config) =>
    {
        config
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json",
                         optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()   // přepíše hodnoty z appsettings env proměnnými
            .AddCommandLine(args);
    })
    .ConfigureServices((ctx, services) =>
    {
        // Konfigurace
        services.Configure<AppSettings>(ctx.Configuration.GetSection("TeamsBot"));

        // Služby
        services.AddSingleton<AuthService>();
        services.AddSingleton<GraphService>();
        services.AddSingleton<SummaryService>();

        // Background worker
        services.AddHostedService<TranscriptWatcher>();
    })
    .Build();

await host.RunAsync();
