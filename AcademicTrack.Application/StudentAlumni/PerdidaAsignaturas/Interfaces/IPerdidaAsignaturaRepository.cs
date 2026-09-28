using AcademicTrack.Domain.Entities;

namespace AcademicTrack.Application.StudentAlumni.PerdidaAsignaturas.Interfaces;

public interface IPerdidaAsignaturaRepository
{
    Task<IReadOnlyList<PerdidaAsignatura>> ObtenerPorPeriodoAsync(
        int programaId,
        int periodoId,
        CancellationToken cancellationToken = default);
}