using AcademicTrack.Application.StudentAlumni.PerdidaAsignaturas.DTOs;
using AcademicTrack.Application.StudentAlumni.PerdidaAsignaturas.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcademicTrack.API.Controllers;

[ApiController]
[Route("api/perdida-asignaturas")]
public class PerdidaAsignaturaController : ControllerBase
{
    private readonly PerdidaAsignaturaService _service;

    public PerdidaAsignaturaController(
        PerdidaAsignaturaService service)
    {
        _service = service;
    }

    [HttpGet("{programaId:int}/{periodoId:int}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<PerdidaAsignaturaDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<PerdidaAsignaturaDto>>> Obtener(
        int programaId,
        int periodoId,
        CancellationToken cancellationToken)
    {
        try
        {
            var resultado = await _service.ObtenerPorPeriodoAsync(
                programaId,
                periodoId,
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

    [HttpGet("analisis/{programaId:int}/{periodoId:int}")]
    [ProducesResponseType(
        typeof(AnalisisPerdidaAsignaturaDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AnalizarPeriodo(
        int programaId,
        int periodoId,
        CancellationToken cancellationToken)
    {
        try
        {
            var resultado = await _service.AnalizarPeriodoAsync(
                programaId,
                periodoId,
                cancellationToken);

            if (resultado is null)
            {
                return NotFound(new
                {
                    mensaje = "No se encontraron datos de pérdida de asignaturas para el periodo."
                });
            }

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
}