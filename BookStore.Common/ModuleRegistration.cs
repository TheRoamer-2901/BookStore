using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BookStore.Common;

public static class ModuleRegistration
{
    public static IServiceCollection RegisterCommonServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLoggingConfig(configuration);
        services.AddMappings();
        return services;
    }

    private static void AddLoggingConfig(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .CreateLogger();
        
        services.AddSingleton(Log.Logger);
    }

    private static void AddMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}