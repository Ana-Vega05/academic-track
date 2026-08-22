namespace AcademicTrack.Domain.Entities;

public class Programa
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string? CodigoSnies { get; set; }
    public string Facultad { get; set; }
    public bool Activo { get; set; } = true;
}