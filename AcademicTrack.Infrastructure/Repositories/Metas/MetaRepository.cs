using AcademicTrack.Application.Metas.Interfaces;
using AcademicTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AcademicTrack.Infrastructure.Persistence;

namespace AcademicTrack.Infrastructure.Repositories.Metas;

public class MetaRepository: IMetaRepository
{
    private readonly AcademicTrackDbContext _context;
    public MetaRepository(AcademicTrackDbContext context) => _context = context;

    public async Task<IReadOnlyList<Meta>> ObtenerPorProgramaAsync(int programaId, CancellationToken cancellationToken = default)
        => await _context.Metas.AsNoTracking()
            .Where(x => x.ProgramaId == programaId)
            .OrderBy(x => x.FechaLimite)
            .ToListAsync(cancellationToken);
    // Sin AsNoTracking a propósito: ActualizarAvanceAsync necesita la entidad rastreada para poder guardarla.
    public async Task<Meta?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Metas.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Meta> CrearAsync(Meta meta, CancellationToken cancellationToken = default)
    {
        _context.Metas.Add(meta);
        await _context.SaveChangesAsync(cancellationToken);
        return meta;
    }

    public async Task ActualizarAsync(Meta meta, CancellationToken cancellationToken = default)
    {
        _context.Metas.Update(meta);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<IReadOnlyList<Meta>> ObtenerTodasAsync(CancellationToken cancellationToken = default)
        => await _context.Metas.AsNoTracking()
            .OrderBy(x => x.ProgramaId)
            .ThenBy(x => x.FechaLimite)
            .ToListAsync(cancellationToken);
    
    public async Task<(IReadOnlyList<Meta> Items, int TotalItems)> ObtenerPaginadoAsync(
        int? programaId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Metas.AsNoTracking().AsQueryable();
        if (programaId is not null)
            query = query.Where(x => x.ProgramaId == programaId);

        query = query.OrderBy(x => x.ProgramaId).ThenBy(x => x.FechaLimite);

        var total = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return (items, total);
    }
}