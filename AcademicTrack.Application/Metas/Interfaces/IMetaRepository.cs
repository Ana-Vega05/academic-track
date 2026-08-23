using AcademicTrack.Domain.Entities;

namespace AcademicTrack.Application.Metas.Interfaces;

public interface IMetaRepository
{
    Task<IReadOnlyList<Meta>> ObtenerPorProgramaAsync(int programaId, CancellationToken cancellationToken = default);
    Task<Meta?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Meta> CrearAsync(Meta meta, CancellationToken cancellationToken = default);
    Task ActualizarAsync(Meta meta, CancellationToken cancellationToken = default);
}