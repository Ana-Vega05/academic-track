using AcademicTrack.Domain.Enums;

namespace AcademicTrack.Domain.Entities;

public class Meta
{
    public int Id { get; set; }
    public int ProgramaId { get; set; }
    public int IndicadorId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string Responsable { get; set; } = string.Empty;
    public PeriodicidadMeta Periodicidad { get; set; }
    public DateOnly FechaInicio { get; set; }
    public DateOnly FechaLimite { get; set; }
    public decimal ValorInicial { get; set; }
    public decimal ValorEsperado { get; set; }
    public decimal AvanceActual { get; set; }
    public EstadoMeta Estado { get; set; } = EstadoMeta.NoIniciada;
}