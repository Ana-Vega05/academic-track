using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AcademicTrack.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
                               ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'Default'.");

        services.AddDbContext<AcademicTrackDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}