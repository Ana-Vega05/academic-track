namespace AcademicTrack.Domain.Entities;

public class Programa
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? CodigoSnies { get; set; }
    public string Facultad { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
}