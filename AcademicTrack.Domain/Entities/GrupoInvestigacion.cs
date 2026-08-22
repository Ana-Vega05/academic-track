namespace AcademicTrack.Domain.Entities;

public class GrupoInvestigacion
{
    public int Id { get; set; }
    public int ProgramaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Sigla { get; set; }
    public string? ClasificacionMinciencias { get; set; } // "A1", "A", "B", "C", "D"
    public short? NumInvestigadores { get; set; }
    public int? TotalProductos { get; set; }
    public int? ArticulosIndexados { get; set; }
    public int? ArticulosRii { get; set; }
    public int? ArticulosRini { get; set; }
    public int? ArticulosRni { get; set; }
    public int? ArticulosRnni { get; set; }
    public int? LibrosCompletos { get; set; }
    public int? LibrosCapitulos { get; set; }
    public int? TrabajosGradoPregrado { get; set; }
    public int? TrabajosGradoMaestria { get; set; }
    public int? TrabajosGradoDoctorado { get; set; }
    public int? NumPatentes { get; set; }
    public int? OtrosResultados { get; set; }
}