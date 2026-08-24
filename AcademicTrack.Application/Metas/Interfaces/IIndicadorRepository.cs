using AcademicTrack.Domain.Entities;

namespace AcademicTrack.Application.Metas.Interfaces;

public interface IIndicadorRepository
{
    Task<Indicador?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Indicador>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
}