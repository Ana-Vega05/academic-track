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

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(UploadIndicatorResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadIndicator(
        [FromForm] string indicatorType,
        [FromForm] string? program,
        [FromForm] string? period,
        IFormFile file,
        CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "Debe adjuntar un archivo válido para procesar el indicador." });
        }

        using var stream = file.OpenReadStream();
        var result = await _service.UploadIndicatorFileAsync(
            indicatorType,
            program ?? "Ingeniería de Sistemas",
            period ?? "2025-1",
            stream,
            file.FileName,
            file.Length,
            cancellationToken
        );
        return Ok(result);
    }
}
