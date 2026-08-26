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
using AcademicTrack.Infrastructure.Persistence;
using AcademicTrack.Infrastructure.Repositories.Programs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
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

// Automatically apply pending EF Core migrations on startup if database server is reachable
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
            "PostgreSQL no está disponible en 127.0.0.1:5432 ({Message}). La aplicación continuará funcionando en modo memoria/lectura.",
            ex.Message);
    }
}

app.UseMiddleware<AcademicTrack.API.Middlewares.ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();