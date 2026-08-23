namespace AcademicTrack.Domain.Entities;

public class MetaEvidencia
{
    public int Id { get; set; }
    public int MetaId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string? Url { get; set; }
    public DateOnly FechaCarga { get; set; }
}