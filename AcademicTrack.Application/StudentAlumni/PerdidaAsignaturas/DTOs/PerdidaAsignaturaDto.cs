namespace AcademicTrack.Application.StudentAlumni.PerdidaAsignaturas.DTOs;

public class PerdidaAsignaturaDto
{
    public int Id { get; init; }

    public int ProgramaId { get; init; }

    public int PeriodoId { get; init; }

    public string Asignatura { get; init; } = string.Empty;

    public int Matriculados { get; init; }

    public int Aprobados { get; init; }

    public int Reprobados { get; init; }

    public decimal PorcentajePerdida { get; init; }
}