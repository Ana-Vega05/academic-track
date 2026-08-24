using AcademicTrack.Application.Metas.Interfaces;
using AcademicTrack.Domain.Entities;
using AcademicTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AcademicTrack.Infrastructure.Repositories.Metas;

public class MetaEvidenciaRepository: IMetaEvidenciaRepository
{
    private readonly AcademicTrackDbContext _context;
    public MetaEvidenciaRepository(AcademicTrackDbContext context) => _context = context;
    
    public async Task<IReadOnlyList<MetaEvidencia>> ObtenerPorMetaAsync(int metaId, CancellationToken cancellationToken = default)
        => await _context.MetaEvidencias.AsNoTracking()
            .Where(x => x.MetaId == metaId)
            .OrderByDescending(x => x.FechaCarga)
            .ToListAsync(cancellationToken);

    public async Task AgregarAsync(MetaEvidencia evidencia, CancellationToken cancellationToken = default)
    {
        _context.MetaEvidencias.Add(evidencia);
        await _context.SaveChangesAsync(cancellationToken);
    }
}