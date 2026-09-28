namespace AcademicTrack.Application.Programs.DTOs;

public class PeriodoDto
{
    public int Id { get; init; }
    public short Anio { get; init; }
    public string Semestre { get; init; } = string.Empty;
}