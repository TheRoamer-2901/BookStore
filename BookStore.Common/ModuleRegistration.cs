using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BookStore.Common;

public static class ModuleRegistration
{
    public static IServiceCollection RegisterCommonServices(this IServiceCollection services)
    {
        services.AddLoggingConfig();
        return services;
    }

    private static void AddLoggingConfig(this IServiceCollection services)
    {
        var projectRoot =  Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName ?? AppDomain.CurrentDomain.BaseDirectory;
        var logFilePath = Path.Combine(projectRoot, "logs", "log.txt");
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(logFilePath)
            .CreateLogger();
        services.AddSingleton(Log.Logger);
    }
}