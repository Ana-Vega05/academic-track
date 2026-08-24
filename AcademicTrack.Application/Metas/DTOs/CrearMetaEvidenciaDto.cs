namespace AcademicTrack.Application.Metas.DTOs;

public class CrearMetaEvidenciaDto
{
    public string Descripcion { get; init; } = string.Empty;
    public string? Url { get; init; }
    public DateOnly? FechaCarga { get; init; } // si no la mandan, se usa la fecha actual
}