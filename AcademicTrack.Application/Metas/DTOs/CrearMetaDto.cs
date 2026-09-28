namespace AcademicTrack.Application.Metas.DTOs;

public class CrearMetaDto
{
    public int ProgramaId { get; init; }
    public int IndicadorId { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string? Descripcion { get; init; }
    public string Responsable { get; init; } = string.Empty;
    public string Periodicidad { get; init; } = string.Empty;
    public DateOnly FechaInicio { get; init; }
    public DateOnly FechaLimite { get; init; }
    public decimal ValorInicial { get; init; }
    public decimal ValorEsperado { get; init; }
}