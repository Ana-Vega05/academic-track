namespace AcademicTrack.Domain.Entities;

public class LineaInvestigacion
{
    public int Id { get; set; }
    public int GrupoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
}