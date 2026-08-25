using AcademicTrack.Application.Programs.DTOs;
using AcademicTrack.Application.Programs.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AcademicTrack.API.Controllers;

[ApiController]
[Route("api/programas")]
public class ProgramaController : ControllerBase
{
    private readonly IProgramaService _service;

    public ProgramaController(IProgramaService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyList<ProgramaDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ProgramaDto>>> Obtener(
        CancellationToken cancellationToken)
    {
        var resultado = await _service.ObtenerActivosAsync(
            cancellationToken);

        return Ok(resultado);
    }
}