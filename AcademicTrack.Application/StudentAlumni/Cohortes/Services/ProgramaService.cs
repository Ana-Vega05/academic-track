using AcademicTrack.Application.Programs.DTOs;
using AcademicTrack.Application.Programs.Interfaces;

namespace AcademicTrack.Application.Programs.Services;

public class ProgramaService : IProgramaService
{
    private readonly IProgramaRepository _repository;

    public ProgramaService(IProgramaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<ProgramaDto>> ObtenerActivosAsync(
        CancellationToken cancellationToken = default)
    {
        var programas = await _repository.ObtenerActivosAsync(
            cancellationToken);

        return programas
            .Select(programa => new ProgramaDto
            {
                Id = programa.Id,
                Nombre = programa.Nombre,
                Facultad = programa.Facultad,
                Activo = programa.Activo
            })
            .ToList();
    }
}