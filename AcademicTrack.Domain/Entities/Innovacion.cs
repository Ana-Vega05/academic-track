namespace AcademicTrack.Domain.Entities;

public class Innovacion
{
    public int Id { get; set; }
    public int ProgramaId { get; set; }
    public string Profesor { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public DateOnly FechaEntrega { get; set; }
    public string EntidadBeneficiada { get; set; } = string.Empty;
    public string? ComunidadBeneficiaria { get; set; }
    public string? Impacto { get; set; }
    public bool TieneSoporte { get; set; } = false;
    public string? AplicacionUso { get; set; }
    public short Anio { get; set; }
}