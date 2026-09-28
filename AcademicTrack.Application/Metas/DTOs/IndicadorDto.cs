namespace AcademicTrack.Application.Metas.DTOs;

public class IndicadorDto
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string? Unidad { get; init; }
    public string Direccion { get; init; } = string.Empty; // Ascendente | Descendente
}