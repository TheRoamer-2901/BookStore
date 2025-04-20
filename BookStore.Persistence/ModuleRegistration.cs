using BookStore.Common;
using BookStore.Contract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Persistence;

public static class ModuleRegistration
{
    private enum RepoKey
    {
        File,
        Db
    }
    
    public static IServiceCollection RegisterPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var isDbStorageEnabled = configuration.GetValue<bool>("FeatureFlags:IsDbStorageEnabled");
        if (isDbStorageEnabled)
        {
            RegisterDbRepository(services, configuration);
        }
        else
        {
            RegisterFileRepository(services, configuration);
        }
        
        services.AddScoped<IBookStoreRepository>(sp =>
        {
            var injectionKey = isDbStorageEnabled 
                ? RepoKey.Db 
                : RepoKey.File;
            
            var innerRepo = sp.GetRequiredKeyedService<IBookStoreRepository>(injectionKey);
            var cache = sp.GetRequiredService<IMemoryCache>();
            return new CachedBookStoreRepository(innerRepo, cache);
        });
        
        return services;
    }

    private static void RegisterDbRepository(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<BookStoreDbContext>(
            options => options.UseNpgsql(connectionString));
        services.AddKeyedScoped<IBookStoreRepository, BookStoreDbRepository>(RepoKey.Db);
    }

    private static void RegisterFileRepository(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BookStoreConfig>(configuration.GetSection("BookStoreSettings"));
        services.AddKeyedSingleton<IBookStoreRepository, BookStoreRepository>(RepoKey.File);
    }
}