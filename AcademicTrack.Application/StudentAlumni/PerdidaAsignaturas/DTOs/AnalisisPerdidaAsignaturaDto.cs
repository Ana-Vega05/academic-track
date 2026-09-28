namespace AcademicTrack.Application.StudentAlumni.PerdidaAsignaturas.DTOs;

public class AnalisisPerdidaAsignaturaDto
{
    public int ProgramaId { get; init; }

    public int PeriodoId { get; init; }

    public int TotalAsignaturas { get; init; }

    public int TotalMatriculados { get; init; }

    public int TotalAprobados { get; init; }

    public int TotalReprobados { get; init; }

    public decimal PorcentajePerdidaGeneral { get; init; }

    public string? AsignaturaMayorPerdida { get; init; }

    public decimal MayorPorcentajePerdida { get; init; }

    public IReadOnlyList<PerdidaAsignaturaDto> Asignaturas { get; init; }
        = [];
}