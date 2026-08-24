using AcademicTrack.Domain.Entities;

namespace AcademicTrack.Application.StudentAlumni.Cohortes.Interfaces;

public interface ISeguimientoCohorteRepository
{
    Task<IReadOnlyList<SeguimientoCohorte>> ObtenerPorCohorteAsync(
        int programaId,
        int periodoCohorteId,
        CancellationToken cancellationToken = default);
}