using AcademicTrack.Application.Programs.Interfaces;
using AcademicTrack.Domain.Entities;
using AcademicTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AcademicTrack.Infrastructure.Repositories.Programs;

public class ProgramaRepository : IProgramaRepository
{
    private readonly AcademicTrackDbContext _context;

    public ProgramaRepository(AcademicTrackDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Programa>> ObtenerActivosAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Programas
            .AsNoTracking()
            .Where(programa => programa.Activo)
            .OrderBy(programa => programa.Nombre)
            .ToListAsync(cancellationToken);
    }
}