using AcademicTrack.Application.StudentAlumni.Egresados.DTOs;
using AcademicTrack.Application.StudentAlumni.Egresados.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcademicTrack.API.Controllers;

[ApiController]
[Route("api/seguimiento-egresado")]
public class SeguimientoEgresadoController : ControllerBase
{
    private readonly SeguimientoEgresadoService _service;

    public SeguimientoEgresadoController(
        SeguimientoEgresadoService service)
    {
        _service = service;
    }

    [HttpGet("{programaId:int}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<SeguimientoEgresadoDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<SeguimientoEgresadoDto>>> ObtenerPorPrograma(
        int programaId,
        CancellationToken cancellationToken)
    {
        try
        {
            var resultado = await _service.ObtenerPorProgramaAsync(
                programaId,
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

    [HttpGet("analisis/{programaId:int}/{anioGraduacion:int}")]
    [ProducesResponseType(
        typeof(AnalisisEgresadoDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AnalisisEgresadoDto>> Analizar(
        int programaId,
        short anioGraduacion,
        CancellationToken cancellationToken)
    {
        try
        {
            var resultado = await _service.AnalizarAsync(
                programaId,
                anioGraduacion,
                cancellationToken);

            if (resultado is null)
            {
                return NotFound(new
                {
                    mensaje = "No se encontraron datos de egresados para el año indicado."
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

    [HttpGet("distribuciones/{seguimientoEgresadoId:int}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<DistribucionEgresadoDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<DistribucionEgresadoDto>>> ObtenerDistribuciones(
        int seguimientoEgresadoId,
        CancellationToken cancellationToken)
    {
        try
        {
            var resultado = await _service.ObtenerDistribucionesAsync(
                seguimientoEgresadoId,
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
}