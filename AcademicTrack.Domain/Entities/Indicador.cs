using AcademicTrack.Domain.Enums;

namespace AcademicTrack.Domain.Entities;

public class Indicador
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Unidad { get; set; }
    public DireccionIndicador Direccion { get; set; }
}