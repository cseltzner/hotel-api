using API.Interfaces.Repositories;
using API.Repositories;

namespace API.Extensions.ServiceExtensions;

public static class ApplicationServiceExtensions
{
    /// <summary>
    /// Adds scoped and transient services to program
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Repository services
        services.AddScoped<IFloorRepository, FloorRepository>();

        return services;
    }
}