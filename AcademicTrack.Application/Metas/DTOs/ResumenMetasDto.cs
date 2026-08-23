namespace AcademicTrack.Application.Metas.DTOs;

public class ResumenMetasDto
{
    public IReadOnlyList<ResumenProgramaDto> PorPrograma { get; init; } = [];
}