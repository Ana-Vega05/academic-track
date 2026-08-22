namespace AcademicTrack.Domain.Entities;

public class ConvenioVinculacion
{
    public int Id { get; set; }
    public int ConvenioId { get; set; }
    public string TipoActor { get; set; } = string.Empty; // "Estudiante", "Docente", "Egresado"
    public short Cantidad { get; set; } = 0;
}