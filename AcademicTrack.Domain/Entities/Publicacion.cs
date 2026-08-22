namespace AcademicTrack.Domain.Entities;

public class Publicacion
{
    public int Id { get; set; }
    public int GrupoId { get; set; }
    public string TipoPublicacion { get; set; } = string.Empty;
    public short Anio { get; set; }
    public string ReferenciaCompleta { get; set; } = string.Empty;
    public string? Url { get; set; }
    public bool? Indexada { get; set; }
    public string? BaseIndexacion { get; set; }
    public string? Cuartil { get; set; }
}