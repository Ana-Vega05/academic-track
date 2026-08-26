using AcademicTrack.Application.Services;
using AcademicTrack.Domain.Enums;
using AcademicTrack.Domain.Models.Requests;
using AcademicTrack.Domain.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AcademicTrack.API.Controllers;

[ApiController]
[Route("activities")]
public class ActivitiesController(ActivityService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] int? programId, [FromQuery] ActivityType? type, CancellationToken cancellationToken = default)
    {
        return Ok(await service.Get(programId, type, cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var activity = await service.GetById(id, cancellationToken);
        return activity is null ? NotFound() : Ok(activity);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateActivityRequest request, CancellationToken cancellationToken)
    {
        var activity = await service.Create(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = activity.Id }, activity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateActivityRequest request, CancellationToken cancellationToken)
    {
        var updated = await service.Update(id, request, cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await service.Delete(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPost("{id:int}/evidences")]
    public async Task<IActionResult> AddEvidence(int id, [FromBody] CreateActivityEvidenceRequest dto, CancellationToken cancellationToken)
    {
        var updated = await service.AddEvidence(id, dto, cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }
}
