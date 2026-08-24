using AcademicTrack.Application.StudentAlumni.Cohortes.DTOs;

namespace AcademicTrack.Application.StudentAlumni.Cohortes.Interfaces;

public interface ISeguimientoCohorteService
{
    Task<IReadOnlyList<SeguimientoCohorteDto>> ObtenerPorCohorteAsync(
        int programaId,
        int periodoCohorteId);

    Task<AnalisisCohorteDto?> AnalizarCohorteAsync(
        int programaId,
        int periodoCohorteId);
}