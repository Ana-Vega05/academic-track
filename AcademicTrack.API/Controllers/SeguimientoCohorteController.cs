using AcademicTrack.Application.StudentAlumni.Cohortes.DTOs;
using AcademicTrack.Application.StudentAlumni.Cohortes.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcademicTrack.API.Controllers;

[ApiController]
[Route("api/seguimiento-cohorte")]
public class SeguimientoCohorteController : ControllerBase
{
    private readonly SeguimientoCohorteService _service;

    public SeguimientoCohorteController(
        SeguimientoCohorteService service)
    {
        _service = service;
    }

    [HttpGet("{programaId:int}/{periodoCohorteId:int}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<SeguimientoCohorteDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<SeguimientoCohorteDto>>> Obtener(
        int programaId,
        int periodoCohorteId,
        CancellationToken cancellationToken)
    {
        try
        {
            var resultado = await _service.ObtenerPorCohorteAsync(
                programaId,
                periodoCohorteId,
                cancellationToken);

            return Ok(resultado);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                mensaje = exception.Message
            });
        }
    }

    [HttpGet("analisis/{programaId:int}/{periodoCohorteId:int}")]
public async Task<IActionResult> AnalizarCohorte(
    int programaId,
    int periodoCohorteId)
{
    var resultado = await _service.AnalizarCohorteAsync(
        programaId,
        periodoCohorteId);

    if (resultado is null)
    {
        return NotFound(new
        {
            mensaje = "No se encontraron datos para la cohorte."
        });
    }

    return Ok(resultado);
}


}