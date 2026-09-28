namespace AcademicTrack.Application.Metas.DTOs;

public class MetaDto
{
    public int Id { get; init; }
    public int ProgramaId { get; init; }
    public string IndicadorNombre { get; init; } = string.Empty;
    public string Nombre { get; init; } = string.Empty;
    public string? Descripcion { get; init; }
    public string Responsable { get; init; } = string.Empty;
    public string Periodicidad { get; init; } = string.Empty;
    public DateOnly FechaInicio { get; init; }
    public DateOnly FechaLimite { get; init; }
    public decimal ValorInicial { get; init; }
    public decimal ValorEsperado { get; init; }
    public decimal AvanceActual { get; init; }
    public string Estado { get; init; } = string.Empty;
    public decimal PorcentajeCumplimiento { get; init; }
    public string Semaforo { get; init; } = string.Empty; // Verde | Amarillo | Rojo | Gris
    public IReadOnlyList<MetaEvidenciaDto> Evidencias { get; init; } = [];
}