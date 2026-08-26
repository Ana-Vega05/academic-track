using AcademicTrack.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademicTrack.API.Controllers;

[ApiController]
[Route("api/programas")]
public class ProgramasController : ControllerBase
{
    private readonly AcademicTrackDbContext _dbContext;

    public ProgramasController(AcademicTrackDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetProgramas(CancellationToken cancellationToken)
    {
        try
        {
            var dbProgramas = await _dbContext.Programas
                .AsNoTracking()
                .Select(p => new
                {
                    id = p.Id,
                    nombre = p.Nombre,
                    facultad = "Facultad de Ingeniería y Tecnologías",
                    activo = p.Activo
                })
                .ToListAsync(cancellationToken);

            if (dbProgramas.Count > 0)
            {
                return Ok(dbProgramas);
            }
        }
        catch
        {
            // Fallback if DB table empty or initializing
        }

        var defaultProgramas = new[]
        {
            new { id = 1, nombre = "Ingeniería de Sistemas", facultad = "Facultad de Ingeniería y Tecnologías", activo = true },
            new { id = 2, nombre = "Ingeniería Industrial", facultad = "Facultad de Ingeniería y Tecnologías", activo = true },
            new { id = 3, nombre = "Ingeniería Electrónica", facultad = "Facultad de Ingeniería y Tecnologías", activo = true }
        };

        return Ok(defaultProgramas);
    }
}
