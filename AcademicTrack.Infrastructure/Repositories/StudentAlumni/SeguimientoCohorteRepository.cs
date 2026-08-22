using AcademicTrack.Application.StudentAlumni.Cohortes.Interfaces;
using AcademicTrack.Domain.Entities;
using AcademicTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AcademicTrack.Infrastructure.Repositories.StudentAlumni;

public class SeguimientoCohorteRepository : ISeguimientoCohorteRepository
{
    private readonly AcademicTrackDbContext _context;

    public SeguimientoCohorteRepository(
        AcademicTrackDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<SeguimientoCohorte>> ObtenerPorCohorteAsync(
        int programaId,
        int periodoCohorteId,
        CancellationToken cancellationToken = default)
    {
        return await _context.SeguimientosCohorte
            .AsNoTracking()
            .Where(x =>
                x.ProgramaId == programaId &&
                x.PeriodoCohorteId == periodoCohorteId)
            .OrderBy(x => x.SemestreSeguimiento)
            .ToListAsync(cancellationToken);
    }
}