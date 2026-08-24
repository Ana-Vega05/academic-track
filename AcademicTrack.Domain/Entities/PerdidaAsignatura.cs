namespace AcademicTrack.Domain.Entities;

public class PerdidaAsignatura
{
    public int Id { get; set; }

    public int ProgramaId { get; set; }

    public int PeriodoId { get; set; }

    public string Asignatura { get; set; } = string.Empty;

    public int Matriculados { get; set; }

    public int Aprobados { get; set; }

    public int Reprobados { get; set; }

    public decimal PorcentajePerdida { get; set; }
}