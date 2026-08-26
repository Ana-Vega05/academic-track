using AcademicTrack.Application.StudentAlumni.Egresados.Interfaces;
using AcademicTrack.Domain.Entities;
using AcademicTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AcademicTrack.Infrastructure.Repositories.StudentAlumni;

public class SeguimientoEgresadoRepository : ISeguimientoEgresadoRepository
{
    private readonly AcademicTrackDbContext _context;

    public SeguimientoEgresadoRepository(
        AcademicTrackDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<SeguimientoEgresado>> ObtenerPorProgramaAsync(
        int programaId,
        CancellationToken cancellationToken = default)
    {
        return await _context.SeguimientosEgresado
            .AsNoTracking()
            .Where(x => x.ProgramaId == programaId)
            .OrderBy(x => x.AnioGraduacion)
            .ToListAsync(cancellationToken);
    }

    public async Task<SeguimientoEgresado?> ObtenerPorProgramaYAnioAsync(
        int programaId,
        short anioGraduacion,
        CancellationToken cancellationToken = default)
    {
        return await _context.SeguimientosEgresado
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x =>
                    x.ProgramaId == programaId &&
                    x.AnioGraduacion == anioGraduacion,
                cancellationToken);
    }

    public async Task<IReadOnlyList<DistribucionEgresado>> ObtenerDistribucionesAsync(
        int seguimientoEgresadoId,
        CancellationToken cancellationToken = default)
    {
        return await _context.DistribucionesEgresado
            .AsNoTracking()
            .Where(x => x.SeguimientoEgresadoId == seguimientoEgresadoId)
            .OrderBy(x => x.Tipo)
            .ThenBy(x => x.Categoria)
            .ToListAsync(cancellationToken);
    }
}