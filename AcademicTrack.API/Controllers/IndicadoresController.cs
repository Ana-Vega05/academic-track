using AcademicTrack.Application.Metas.DTOs;
using AcademicTrack.Application.Metas.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AcademicTrack.API.Controllers;

[ApiController]
[Route("api/indicadores")]
public class IndicadoresController: ControllerBase
{
    private readonly IIndicadorRepository _repository;
    public IndicadoresController(IIndicadorRepository repository) => _repository = repository;

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos(CancellationToken cancellationToken)
    {
        var indicadores = await _repository.ObtenerTodosAsync(cancellationToken);
        var dtos = indicadores.Select(i => new IndicadorDto
        {
            Id = i.Id,
            Nombre = i.Nombre,
            Unidad = i.Unidad,
            Direccion = i.Direccion.ToString()
        });
        return Ok(dtos);
    }
}