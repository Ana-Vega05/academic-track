using AcademicTrack.Application.Metas.DTOs;
using AcademicTrack.Application.Metas.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcademicTrack.API.Controllers;

[ApiController]
[Route("api/metas")]
public class MetasController: ControllerBase
{
    private readonly MetaService _service;
    public MetasController(MetaService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Obtener(
        [FromQuery] int? programaId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
        => Ok(await _service.ObtenerAsync(programaId, page, pageSize, cancellationToken));
    
    [HttpGet("resumen")]
    public async Task<IActionResult> ObtenerResumen(CancellationToken cancellationToken)
        => Ok(await _service.ObtenerResumenAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id, CancellationToken cancellationToken)
    {
        var meta = await _service.ObtenerPorIdAsync(id, cancellationToken);
        return meta is null ? NotFound() : Ok(meta);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearMetaDto dto, CancellationToken cancellationToken)
    {
        var creada = await _service.CrearAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarMetaDto dto, CancellationToken cancellationToken)
    {
        var actualizada = await _service.ActualizarMetaAsync(id, dto, cancellationToken);
        return actualizada is null ? NotFound() : Ok(actualizada);
    }

    [HttpPatch("{id:int}/avance")]
    public async Task<IActionResult> ActualizarAvance(int id, [FromBody] ActualizarAvanceMetaDto dto, CancellationToken cancellationToken)
    {
        var actualizada = await _service.ActualizarAvanceAsync(id, dto, cancellationToken);
        return actualizada is null ? NotFound() : Ok(actualizada);
    }

    [HttpPatch("{id:int}/cancelar")]
    public async Task<IActionResult> Cancelar(int id, CancellationToken cancellationToken)
    {
        var cancelada = await _service.CancelarAsync(id, cancellationToken);
        return cancelada is null ? NotFound() : Ok(cancelada);
    }

    [HttpPost("{id:int}/evidencias")]
    public async Task<IActionResult> AgregarEvidencia(int id, [FromBody] CrearMetaEvidenciaDto dto, CancellationToken cancellationToken)
    {
        var actualizada = await _service.AgregarEvidenciaAsync(id, dto, cancellationToken);
        return actualizada is null ? NotFound() : Ok(actualizada);
    }
}