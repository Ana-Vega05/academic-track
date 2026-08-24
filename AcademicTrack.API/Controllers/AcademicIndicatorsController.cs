using AcademicTrack.Application.AcademicIndicators.DTOs;
using AcademicTrack.Application.AcademicIndicators.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AcademicTrack.API.Controllers;

[ApiController]
[Route("api/academic-indicators")]
public class AcademicIndicatorsController : ControllerBase
{
    private readonly IAcademicIndicatorsService _service;

    public AcademicIndicatorsController(IAcademicIndicatorsService service)
    {
        _service = service;
    }

    [HttpGet("dashboard")]
    [ProducesResponseType(typeof(AcademicIndicatorsDashboardDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDashboard(
        [FromQuery] string? program,
        [FromQuery] string? period,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetDashboardDataAsync(program, period, cancellationToken);
        return Ok(result);
    }
}
