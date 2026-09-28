using AcademicTrack.Domain.Entities;

namespace AcademicTrack.Application.Programs.Interfaces;

public interface IProgramaRepository
{
    Task<IReadOnlyList<Programa>> ObtenerActivosAsync(
        CancellationToken cancellationToken = default);
}