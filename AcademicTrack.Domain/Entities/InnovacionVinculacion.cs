namespace AcademicTrack.Domain.Entities;

public class InnovacionVinculacion
{
    public int Id { get; set; }
    public int InnovacionId { get; set; }
    public string TipoActor { get; set; } = string.Empty; // "Estudiante", "Docente", "Administrativo"
    public string Condicion { get; set; } = "Interno"; // "Interno" o "Externo"
    public short Cantidad { get; set; } = 0;
}