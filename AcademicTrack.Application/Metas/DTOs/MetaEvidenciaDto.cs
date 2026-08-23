namespace AcademicTrack.Application.Metas.DTOs;

public class MetaEvidenciaDto
{
    public string Descripcion { get; init; } = string.Empty;
    public string? Url { get; init; }
    public DateOnly FechaCarga { get; init; }
}