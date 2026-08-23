using AcademicTrack.Domain.Entities;
using AcademicTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AcademicTrack.Infrastructure.Repositories.Metas;

public class IndicadorRepository
{
    private readonly AcademicTrackDbContext _context;
    public IndicadorRepository(AcademicTrackDbContext context) => _context = context;

    public async Task<Indicador?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Indicadores.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Indicador>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        => await _context.Indicadores.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync(cancellationToken);
}