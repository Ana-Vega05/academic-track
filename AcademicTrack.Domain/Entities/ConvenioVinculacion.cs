using AcademicTrack.Domain.Enums;

namespace AcademicTrack.Domain.Entities;

public class ConvenioVinculacion
{
    public int Id { get; set; }
    public int ConvenioId { get; set; }
    public TipoActor TipoActor { get; set; } // "Estudiante", "Docente", "Egresado"
    public short Cantidad { get; set; } = 0;
}