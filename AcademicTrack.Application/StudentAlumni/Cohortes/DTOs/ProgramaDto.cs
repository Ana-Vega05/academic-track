namespace AcademicTrack.Application.Programs.DTOs;

public class ProgramaDto
{
    public int Id { get; init; }

    public string Nombre { get; init; } = string.Empty;

    public string Facultad { get; init; } = string.Empty;

    public bool Activo { get; init; }
}