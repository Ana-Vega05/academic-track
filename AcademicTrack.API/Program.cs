using AcademicTrack.Application.AcademicIndicators.Interfaces;
using AcademicTrack.Application.AcademicIndicators.Services;
using AcademicTrack.Application.Auth.Interfaces;
using AcademicTrack.Application.Auth.Services;
using AcademicTrack.Application.Metas.Services;
using AcademicTrack.Application.Programs.Interfaces;
using AcademicTrack.Application.Programs.Services;
using AcademicTrack.Application.Services;
using AcademicTrack.Application.StudentAlumni.Cohortes.Interfaces;
using AcademicTrack.Application.StudentAlumni.Cohortes.Services;
using AcademicTrack.Application.StudentAlumni.Egresados.Services;
using AcademicTrack.Application.StudentAlumni.PerdidaAsignaturas.Services;
using AcademicTrack.Infrastructure.Persistence;
using AcademicTrack.Infrastructure.Repositories.Programs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuración de Swagger con soporte para Bearer Token
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AcademicTrack API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticación JWT usando el esquema Bearer. Ejemplo: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configuración de Autenticación JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "AcademicTrack_DefaultSecretKey_For_Jwt_Security_Token_2024!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "AcademicTrackAPI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "AcademicTrackClient";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

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

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AcademicTrack.Application.Roles.Interfaces.IRoleService, AcademicTrack.Infrastructure.Services.RoleService>();

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

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
