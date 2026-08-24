using AcademicTrack.Application.StudentAlumni.Cohortes.DTOs;
using AcademicTrack.Domain.Entities;

namespace AcademicTrack.Application.StudentAlumni.Cohortes.Interfaces;

public interface ISeguimientoCohorteRepository
{
    Task<IReadOnlyList<SeguimientoCohorte>> ObtenerPorCohorteAsync(
        int programaId,
        int periodoCohorteId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SeguimientoCohorte>> ObtenerPorProgramaAsync(
        int programaId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SeguimientoCohorteConPeriodoDto>> ObtenerConPeriodoAsync(
        int programaId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SeguimientoCohorteConPeriodoDto>> ObtenerComparacionAsync(
        int programaId,
        CancellationToken cancellationToken = default);
}