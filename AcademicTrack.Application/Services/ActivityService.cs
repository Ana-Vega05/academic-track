using AcademicTrack.Domain.Entities;
using AcademicTrack.Domain.Enums;
using AcademicTrack.Domain.Exceptions;
using AcademicTrack.Domain.Models.Requests;
using AcademicTrack.Domain.Models.Responses;
using AcademicTrack.Domain.Repositories;

namespace AcademicTrack.Application.Services;

public class ActivityService(
    IActivityRepository activityRepository,
    IActivityEvidenceRepository evidenceRepository)
{
    public async Task<IReadOnlyList<ActivityResponse>> Get(int? programId, ActivityType? type,
        CancellationToken cancellationToken = default)
    {
        var activities = await activityRepository.Get(programId, type, cancellationToken);

        var items = new List<ActivityResponse>();
        foreach (var activity in activities)
            items.Add(await Map(activity, cancellationToken));

        return items;
    }

    public async Task<ActivityResponse?> GetById(int id, CancellationToken cancellationToken = default)
    {
        var activity = await activityRepository.GetById(id, cancellationToken);
        return activity is null ? null : await Map(activity, cancellationToken);
    }

    public async Task<ActivityResponse> Create(CreateActivityRequest dto, CancellationToken cancellationToken = default)
    {
        ActivityExceptions.ValidateActivity(dto.ProgramId, dto.Name, dto.Responsible, dto.Location);

        var activity = new Activity
        {
            ProgramId = dto.ProgramId,
            Type = dto.Type,
            Name = dto.Name,
            Date = dto.Date,
            Location = dto.Location,
            Responsible = dto.Responsible,
            ParticipatingProfessors = dto.ParticipatingProfessors,
            ParticipatingStudents = dto.ParticipatingStudents,
            Description = dto.Description
        };

        var created = await activityRepository.Create(activity, cancellationToken)
            ?? throw new InvalidOperationException("No se pudo crear la actividad.");
        return await Map(created, cancellationToken);
    }

    public async Task<ActivityResponse?> Update(int id, UpdateActivityRequest dto, CancellationToken cancellationToken = default)
    {
        var activity = await activityRepository.GetById(id, cancellationToken);
        if (activity is null) return null;

        ActivityExceptions.ValidateActivity(activity.ProgramId, dto.Name, dto.Responsible, dto.Location);

        activity.Name = dto.Name;
        activity.Date = dto.Date;
        activity.Location = dto.Location;
        activity.Responsible = dto.Responsible;
        activity.ParticipatingProfessors = dto.ParticipatingProfessors;
        activity.ParticipatingStudents = dto.ParticipatingStudents;
        activity.Description = dto.Description;

        await activityRepository.Update(activity, cancellationToken);
        return await Map(activity, cancellationToken);
    }

    public async Task<bool> Delete(int id, CancellationToken cancellationToken = default)
    {
        var activity = await activityRepository.GetById(id, cancellationToken);
        if (activity is null) return false;

        await activityRepository.Delete(activity, cancellationToken);
        return true;
    }

    public async Task<ActivityResponse?> AddEvidence(int activityId, CreateActivityEvidenceRequest dto, CancellationToken cancellationToken = default)
    {
        var activity = await activityRepository.GetById(activityId, cancellationToken);
        if (activity is null) return null;

        if (string.IsNullOrWhiteSpace(dto.Url))
            throw new ArgumentException("La URL de la evidencia es obligatoria.", nameof(dto.Url));
        ActivityExceptions.ValidateLength(dto.Url, 500, nameof(dto.Url));

        var evidence = new ActivityEvidence
        {
            ActivityId = activityId,
            Url = dto.Url,
            UploadDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        await evidenceRepository.Add(evidence, cancellationToken);
        return await Map(activity, cancellationToken);
    }

    private async Task<ActivityResponse> Map(Activity activity, CancellationToken cancellationToken)
    {
        var evidences = await evidenceRepository.GetByActivity(activity.Id, cancellationToken);

        return new ActivityResponse
        {
            Id = activity.Id,
            ProgramId = activity.ProgramId,
            Type = activity.Type,
            Name = activity.Name,
            Date = activity.Date,
            Location = activity.Location,
            Responsible = activity.Responsible,
            ParticipatingProfessors = activity.ParticipatingProfessors,
            ParticipatingStudents = activity.ParticipatingStudents,
            Description = activity.Description,
            Evidences = evidences
                .Select(e => new ActivityEvidenceResponse { Url = e.Url, UploadDate = e.UploadDate })
                .ToList()
        };
    }
}
