using AcademicTrack.Domain.Enums;

namespace AcademicTrack.Domain.Entities;

public class Convenio
{
    public int Id { get; set; }
    public int ProgramaId { get; set; }
    public string CiudadOPais { get; set; } = string.Empty;
    public string Institucion { get; set; } = string.Empty;
    public string Objeto { get; set; } = string.Empty;
    public string? LogroResultados { get; set; }
    public int? NumUsuarios { get; set; }
    public string? Vigencia { get; set; }
    public TipoConvenio Tipo { get; set; } // "Nacional" / "Internacional"
    public EstadoConvenio Estado { get; set; } // "Activo" / "Inactivo"
}