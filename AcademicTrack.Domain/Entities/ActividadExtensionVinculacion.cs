namespace AcademicTrack.Domain.Entities;

public class ActividadExtensionVinculacion
{
    public int Id { get; set; }
    public int ActividadId { get; set; }
    public string TipoActor { get; set; } = string.Empty; // "Estudiante", "Docente", "Egresado", "Administrativo"
    public short Cantidad { get; set; } = 0;
}