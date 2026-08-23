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
    public async Task<IActionResult> ObtenerPorPrograma([FromQuery] int programaId, CancellationToken cancellationToken)
        => Ok(await _service.ObtenerPorProgramaAsync(programaId, cancellationToken));
    
    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearMetaDto dto, CancellationToken cancellationToken)
    {
        var creada = await _service.CrearAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorPrograma), new { programaId = creada.ProgramaId }, creada);
    }

    [HttpPatch("{id:int}/avance")]
    public async Task<IActionResult> ActualizarAvance(int id, [FromBody] ActualizarAvanceMetaDto dto, CancellationToken cancellationToken)
    {
        var actualizada = await _service.ActualizarAvanceAsync(id, dto, cancellationToken);
        return actualizada is null ? NotFound() : Ok(actualizada);
    }
}