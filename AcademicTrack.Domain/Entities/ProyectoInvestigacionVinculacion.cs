using AcademicTrack.Domain.Enums;

namespace AcademicTrack.Domain.Entities;

public class ProyectoInvestigacionVinculacion
{
    public int Id { get; set; }
    public int ProyectoId { get; set; }
    public TipoActor TipoActor { get; set; }
    public short Cantidad { get; set; } = 0;
}