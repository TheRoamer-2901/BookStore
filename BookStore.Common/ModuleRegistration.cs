using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace BookStore.Common;

public static class ModuleRegistration
{
    public static IServiceCollection RegisterCommonServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLoggingConfig(configuration);
        services.AddMappings();
        services.AddCache(configuration);
        return services;
    }

    private static void AddLoggingConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var path = Path.Combine(PathUtils.ProjectRootPath(), "BookStore", "logs", "log.txt");
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .WriteTo.File(
                Path.Combine(PathUtils.ProjectRootPath(), "BookStore", "logs", "log.txt"),
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Information
            )
            .CreateLogger();
        
        services.AddSingleton(Log.Logger);
    }

    private static void AddMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
    
    private static void AddCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache(options =>
        {
            var sizeLimit = configuration.GetValue<long?>("CacheConfig:SizeLimit");
            if (sizeLimit.HasValue)
            {
                options.SizeLimit = sizeLimit;
            }        
        });
    }
}