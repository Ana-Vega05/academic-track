namespace AcademicTrack.Application.Metas.DTOs;

public class ActualizarAvanceMetaDto
{
    public decimal AvanceActual { get; init; }
    public string? Estado { get; init; } // opcional: si no lo mandan, el service lo infiere
}