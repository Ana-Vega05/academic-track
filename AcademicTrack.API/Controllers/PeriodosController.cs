using AcademicTrack.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademicTrack.API.Controllers;

[ApiController]
[Route("api/periodos")]
public class PeriodosController : ControllerBase
{
    private readonly AcademicTrackDbContext _dbContext;

    public PeriodosController(AcademicTrackDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetPeriodos(CancellationToken cancellationToken)
    {
        try
        {
            var dbPeriodos = await _dbContext.PeriodosAcademicos
                .AsNoTracking()
                .OrderByDescending(p => p.Anio)
                .ThenByDescending(p => p.Semestre)
                .Select(p => new
                {
                    id = p.Id,
                    anio = p.Anio,
                    semestre = p.Semestre == (short)1 ? "I" : "II"
                })
                .ToListAsync(cancellationToken);

            if (dbPeriodos.Count > 0)
            {
                return Ok(dbPeriodos);
            }
        }
        catch
        {
            // Fallback if DB table empty or initializing
        }

        var defaultPeriodos = new[]
        {
            new { id = 1, anio = 2025, semestre = "I" },
            new { id = 2, anio = 2024, semestre = "II" },
            new { id = 3, anio = 2024, semestre = "I" },
            new { id = 4, anio = 2023, semestre = "II" },
            new { id = 5, anio = 2023, semestre = "I" },
            new { id = 6, anio = 2022, semestre = "II" },
            new { id = 7, anio = 2022, semestre = "I" },
            new { id = 8, anio = 2021, semestre = "II" },
            new { id = 9, anio = 2021, semestre = "I" },
            new { id = 10, anio = 2020, semestre = "II" },
            new { id = 11, anio = 2020, semestre = "I" },
            new { id = 12, anio = 2019, semestre = "II" },
            new { id = 13, anio = 2019, semestre = "I" },
            new { id = 14, anio = 2018, semestre = "II" },
            new { id = 15, anio = 2018, semestre = "I" }
        };

        return Ok(defaultPeriodos);
    }
}
