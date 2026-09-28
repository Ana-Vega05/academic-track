using AcademicTrack.Domain.Entities;
using AcademicTrack.Domain.Repositories;
using Dapper;

namespace AcademicTrack.Infrastructure.Repositories.Activities;

public class ActivityEvidenceRepository: IActivityEvidenceRepository
{
    private readonly IDatabaseUtilities _databaseUtilities;
    public ActivityEvidenceRepository(IDatabaseUtilities databaseUtilities) => _databaseUtilities = databaseUtilities;

    public async Task<IReadOnlyList<ActivityEvidence>> GetByActivity(int activityId, CancellationToken cancellationToken = default)
    {
        const string query = """
            SELECT "Id", "ActivityId", "Url", "UploadDate"
            FROM activity_evidence
            WHERE "ActivityId" = @ActivityId
            ORDER BY "UploadDate" DESC
            """;

        var parameters = new DynamicParameters(new { ActivityId = activityId });
        return await _databaseUtilities.ExecuteQuery<ActivityEvidence>(query, parameters);
    }

    public async Task Add(ActivityEvidence evidence, CancellationToken cancellationToken = default)
    {
        const string command = """
            INSERT INTO activity_evidence ("ActivityId", "Url", "UploadDate")
            VALUES (@ActivityId, @Url, @UploadDate)
            """;

        await _databaseUtilities.ExecuteCommandAsync(command, new
        {
            evidence.ActivityId,
            evidence.Url,
            evidence.UploadDate
        });
    }
}
