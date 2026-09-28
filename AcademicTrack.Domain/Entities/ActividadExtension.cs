namespace AcademicTrack.Domain.Entities;

public class ActividadExtension
{
    public int Id { get; set; }
    public int ProgramaId { get; set; }
    public short Anio { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Coordinador { get; set; }
    public string? LogroResultados { get; set; }
    public int ComunidadBeneficiada { get; set; } = 0;
}