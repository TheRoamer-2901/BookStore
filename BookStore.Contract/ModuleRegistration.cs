using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BookStore.Contract;

public static class ModuleRegistration
{
    public static IServiceCollection RegisterContractLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}
