using AcademicTrack.Application.StudentAlumni.PerdidaAsignaturas.Interfaces;
using AcademicTrack.Domain.Entities;
using AcademicTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AcademicTrack.Infrastructure.Repositories.StudentAlumni;

public class PerdidaAsignaturaRepository : IPerdidaAsignaturaRepository
{
    private readonly AcademicTrackDbContext _context;

    public PerdidaAsignaturaRepository(
        AcademicTrackDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<PerdidaAsignatura>> ObtenerPorPeriodoAsync(
        int programaId,
        int periodoId,
        CancellationToken cancellationToken = default)
    {
        return await _context.PerdidasAsignatura
            .AsNoTracking()
            .Where(x =>
                x.ProgramaId == programaId &&
                x.PeriodoId == periodoId)
            .OrderByDescending(x => x.PorcentajePerdida)
            .ThenBy(x => x.Asignatura)
            .ToListAsync(cancellationToken);
    }
}