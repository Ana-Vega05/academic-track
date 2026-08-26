using AcademicTrack.Domain.Entities;
using AcademicTrack.Domain.Enums;

namespace AcademicTrack.Domain.Repositories;

public interface IActivityRepository
{
    Task<IReadOnlyList<Activity>> Get(
        int? programId, ActivityType? type, CancellationToken cancellationToken = default);
    Task<Activity?> GetById(int id, CancellationToken cancellationToken = default);
    Task<Activity?> Create(Activity activity, CancellationToken cancellationToken = default);
    Task Update(Activity activity, CancellationToken cancellationToken = default);
    Task Delete(Activity activity, CancellationToken cancellationToken = default);
}
