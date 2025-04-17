using BookStore.Common;
using BookStore.Contract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Persistence;

public static class ModuleRegistration
{
    public static IServiceCollection RegisterPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterFileRepository(services, configuration);
        RegisterDbRepository(services, configuration);
        return services;
    }

    private static void RegisterDbRepository(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<BookStoreDbContext>(
            options => options.UseNpgsql(connectionString));
        services.AddScoped<IBookStoreDbRepository, BookStoreDbRepository>();
    }

    private static void RegisterFileRepository(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BookStoreConfig>(configuration.GetSection("BookStoreSettings"));
        services.AddScoped<IBookStoreRepository, BookStoreRepository>();
    }
}