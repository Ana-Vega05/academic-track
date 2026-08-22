namespace AcademicTrack.Domain.Entities;

public class PeriodoAcademico
{
    public int Id { get; set; }
    public short Anio { get; set; }
    public string Semestre { get; set; } = string.Empty; // "I" o "II"
}