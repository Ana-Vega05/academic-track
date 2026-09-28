using AcademicTrack.Domain.Entities;

namespace AcademicTrack.Application.StudentAlumni.Egresados.Interfaces;

public interface ISeguimientoEgresadoRepository
{
    Task<IReadOnlyList<SeguimientoEgresado>> ObtenerPorProgramaAsync(
        int programaId,
        CancellationToken cancellationToken = default);

    Task<SeguimientoEgresado?> ObtenerPorProgramaYAnioAsync(
        int programaId,
        short anioGraduacion,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DistribucionEgresado>> ObtenerDistribucionesAsync(
        int seguimientoEgresadoId,
        CancellationToken cancellationToken = default);
}