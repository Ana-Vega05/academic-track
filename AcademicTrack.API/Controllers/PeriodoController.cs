using AcademicTrack.Domain.Entities;
using AcademicTrack.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademicTrack.API.Controllers;

[ApiController]
[Route("api/periodos")]
public class PeriodoController : ControllerBase
{
    private readonly AcademicTrackDbContext _context;

    public PeriodoController(AcademicTrackDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Obtener(
        CancellationToken cancellationToken)
    {
        var periodos = await _context.Set<PeriodoAcademico>()
            .AsNoTracking()
            .OrderByDescending(p => p.Anio)
            .ThenByDescending(p => p.Semestre)
            .Select(p => new
            {
                id = p.Id,
                anio = p.Anio,
                semestre = p.Semestre.ToString()
            })
            .ToListAsync(cancellationToken);

        return Ok(periodos);
    }
}
