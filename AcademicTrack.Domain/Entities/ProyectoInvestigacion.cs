namespace AcademicTrack.Domain.Entities;

public class ProyectoInvestigacion
{
    public int Id { get; set; }
    public int GrupoId { get; set; }
    public int? LineaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string InvestigadorPrincipal { get; set; } = string.Empty;
    public string? ProductosGenerados { get; set; }
    public string? UrlProducto { get; set; }
    public string? ComunidadBeneficiada { get; set; }
    public short? AnioInicio { get; set; }
    public bool Vigente { get; set; } = true;
}