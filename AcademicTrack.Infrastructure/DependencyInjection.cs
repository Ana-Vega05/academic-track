using AcademicTrack.Application.AcademicIndicators.Interfaces;
using AcademicTrack.Application.Metas.Interfaces;
using AcademicTrack.Application.Metas.Services;
using AcademicTrack.Application.StudentAlumni.Cohortes.Interfaces;
using AcademicTrack.Infrastructure.Repositories.AcademicIndicators;
using AcademicTrack.Infrastructure.Repositories.Metas;
using AcademicTrack.Infrastructure.Repositories.StudentAlumni;
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
        
        services.AddScoped<IIndicadorRepository, IndicadorRepository>();
        services.AddScoped<IMetaRepository, MetaRepository>();
        services.AddScoped<IMetaEvidenciaRepository, MetaEvidenciaRepository>();
        services.AddScoped<IAcademicIndicatorsRepository, AcademicIndicatorsRepository>();
        services.AddScoped<MetaService>();

        return services;
    }
}
