using AcademicTrack.Application.AcademicIndicators.Interfaces;
using AcademicTrack.Application.AcademicIndicators.Services;
using AcademicTrack.Application.Metas.Services;
using AcademicTrack.Application.Services;
using AcademicTrack.Infrastructure.Persistence;
using AcademicTrack.Application.StudentAlumni.Cohortes.Interfaces;
using AcademicTrack.Application.StudentAlumni.Cohortes.Services;
using AcademicTrack.Application.StudentAlumni.Egresados.Services;
using AcademicTrack.Application.StudentAlumni.PerdidaAsignaturas.Services;
using AcademicTrack.Application.Programs.Interfaces;
using AcademicTrack.Application.Programs.Services;
using AcademicTrack.Infrastructure.Repositories.Programs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Política CORS universal para permitir peticiones desde localhost y la IP del servidor 169.58.185.218
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ISeguimientoCohorteService, SeguimientoCohorteService>();
builder.Services.AddScoped<SeguimientoEgresadoService>();
builder.Services.AddScoped<PerdidaAsignaturaService>();

builder.Services.AddScoped<IProgramaRepository, ProgramaRepository>();
builder.Services.AddScoped<IProgramaService, ProgramaService>();

builder.Services.AddScoped<MetaService>();
builder.Services.AddScoped<IAcademicIndicatorsService, AcademicIndicatorsService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AcademicTrackDbContext>();
        db.Database.Migrate();
        logger.LogInformation("Migraciones de PostgreSQL aplicadas exitosamente.");

        DbInitializer.Seed(db);
        logger.LogInformation("Sembrado de datos iniciales ejecutado exitosamente.");
    }
    catch (Exception ex)
    {
        logger.LogWarning(
            "PostgreSQL no está disponible ({Message}). La aplicación continuará funcionando.",
            ex.Message);
    }
}

app.UseMiddleware<AcademicTrack.API.Middlewares.ExceptionHandlingMiddleware>();

// Habilitar CORS globalmente
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
