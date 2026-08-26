using AcademicTrack.Application.StudentAlumni.Cohortes.DTOs;
using AcademicTrack.Application.StudentAlumni.Cohortes.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AcademicTrack.API.Controllers;

[ApiController]
[Route("api/seguimiento-cohorte")]
public class SeguimientoCohorteController : ControllerBase
{
    private readonly ISeguimientoCohorteService _service;

        public SeguimientoCohorteController(
        ISeguimientoCohorteService service)
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
        int periodoCohorteId,
        CancellationToken cancellationToken)
    {
        var resultado = await _service.AnalizarCohorteAsync(
            programaId,
            periodoCohorteId,
            cancellationToken);

        if (resultado is null)
        {
            return NotFound(new
            {
                mensaje = "No se encontraron datos para la cohorte."
            });
        }

        return Ok(resultado);
    }




[HttpGet("comparacion/{programaId:int}")]
    public async Task<IActionResult> CompararCohortes(
        int programaId,
        CancellationToken cancellationToken)
    {
        var resultado = await _service.CompararCohortesAsync(
            programaId,
            cancellationToken);

        return Ok(resultado);
    }


}