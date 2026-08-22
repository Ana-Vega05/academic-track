using AcademicTrack.Domain.Enums;

namespace AcademicTrack.Domain.Entities;

public class InnovacionVinculacion
{
    public int Id { get; set; }
    public int InnovacionId { get; set; }
    public TipoActor TipoActor { get; set; } // "Estudiante", "Docente", "Administrativo"
    public CondicionActor Condicion { get; set; } // "Interno" o "Externo"
    public short Cantidad { get; set; } = 0;
}