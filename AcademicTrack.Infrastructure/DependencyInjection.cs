using AcademicTrack.Application.StudentAlumni.Cohortes.Interfaces;
using AcademicTrack.Application.StudentAlumni.Egresados.Interfaces;
using AcademicTrack.Infrastructure.Repositories.StudentAlumni;
using AcademicTrack.Application.StudentAlumni.PerdidaAsignaturas.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AcademicTrack.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "No se encontró la cadena de conexión 'DefaultConnection'.");

        services.AddDbContext<AcademicTrackDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<
            ISeguimientoCohorteRepository,
            SeguimientoCohorteRepository>();

         services.AddScoped<
            ISeguimientoEgresadoRepository,
            SeguimientoEgresadoRepository>();

        services.AddScoped<
            IPerdidaAsignaturaRepository,
            PerdidaAsignaturaRepository>();

return services;
    }
}