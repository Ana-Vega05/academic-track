using AcademicTrack.Application.Programs.DTOs;

namespace AcademicTrack.Application.Programs.Interfaces;

public interface IProgramaService
{
    Task<IReadOnlyList<ProgramaDto>> ObtenerActivosAsync(
        CancellationToken cancellationToken = default);
}