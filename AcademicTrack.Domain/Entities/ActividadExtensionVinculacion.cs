using AcademicTrack.Domain.Enums;

namespace AcademicTrack.Domain.Entities;

public class ActividadExtensionVinculacion
{
    public int Id { get; set; }
    public int ActividadId { get; set; }
    public TipoActor TipoActor { get; set; }
    public short Cantidad { get; set; } = 0;
}