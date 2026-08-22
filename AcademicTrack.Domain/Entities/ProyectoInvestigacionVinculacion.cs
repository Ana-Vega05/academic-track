namespace AcademicTrack.Domain.Entities;

public class ProyectoInvestigacionVinculacion
{
    public int Id { get; set; }
    public int ProyectoId { get; set; }
    public string TipoActor { get; set; } = string.Empty;
    public short Cantidad { get; set; } = 0;
}