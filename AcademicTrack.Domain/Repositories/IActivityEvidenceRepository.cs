using AcademicTrack.Domain.Entities;

namespace AcademicTrack.Domain.Repositories;

public interface IActivityEvidenceRepository
{
    Task<IReadOnlyList<ActivityEvidence>> GetByActivity(int activityId, CancellationToken cancellationToken = default);
    Task Add(ActivityEvidence evidence, CancellationToken cancellationToken = default);
}
