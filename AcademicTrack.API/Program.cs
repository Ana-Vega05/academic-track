using AcademicTrack.Application.AcademicIndicators.Interfaces;
using AcademicTrack.Application.AcademicIndicators.Services;
using AcademicTrack.Application.Metas.Services;
using AcademicTrack.Application.StudentAlumni.Cohortes.Services;
using AcademicTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<SeguimientoCohorteService>();
builder.Services.AddScoped<MetaService>();
builder.Services.AddScoped<IAcademicIndicatorsService, AcademicIndicatorsService>();

var app = builder.Build();

// Automatically apply pending EF Core migrations on startup
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AcademicTrackDbContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al aplicar las migraciones de la base de datos PostgreSQL.");
    }
}

app.UseMiddleware<AcademicTrack.API.Middlewares.ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();
