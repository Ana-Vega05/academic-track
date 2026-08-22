using AcademicTrack.Domain.Enums;

namespace AcademicTrack.Domain.Entities;

public class PeriodoAcademico
{
    public int Id { get; set; }
    public short Anio { get; set; }
    public Semestre Semestre { get; set; } // "I" o "II"
}