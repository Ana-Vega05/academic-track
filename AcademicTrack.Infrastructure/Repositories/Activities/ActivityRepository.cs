using System.Text.Json;
using AcademicTrack.Domain.Entities;
using AcademicTrack.Domain.Enums;
using AcademicTrack.Domain.Repositories;
using Dapper;

namespace AcademicTrack.Infrastructure.Repositories.Activities;

public class ActivityRepository(IDatabaseUtilities databaseUtilities) : IActivityRepository
{
    private sealed record ActivityRow(
        int Id, int ProgramId, ActivityType Type, string Name, DateOnly Date,
        string? Location, string Responsible, string? ParticipatingProfessors,
        string? ParticipatingStudents, string? Description)
    {
        public Activity ToActivity() => new()
        {
            Id = Id,
            ProgramId = ProgramId,
            Type = Type,
            Name = Name,
            Date = Date,
            Location = Location,
            Responsible = Responsible,
            ParticipatingProfessors = DeserializeList(ParticipatingProfessors),
            ParticipatingStudents = DeserializeList(ParticipatingStudents),
            Description = Description
        };
    }

    private const string Columns = """
                                    "Id", "ProgramId", "Type", "Name", "Date", "Location", "Responsible",
                                    "ParticipatingProfessors", "ParticipatingStudents", "Description"
                                    """;

    public async Task<Activity?> GetById(int id, CancellationToken cancellationToken = default)
    {
        var query = $"""SELECT {Columns} FROM activity WHERE "Id" = @Id""";
        var parameters = new DynamicParameters(new { Id = id });
        var row = await databaseUtilities.ExecuteQuerySingle<ActivityRow>(query, parameters);
        return row?.ToActivity();
    }

    public async Task<Activity?> Create(Activity activity, CancellationToken cancellationToken = default)
    {
        const string query = """
            INSERT INTO activity
                ("ProgramId", "Type", "Name", "Date", "Location", "Responsible",
                 "ParticipatingProfessors", "ParticipatingStudents", "Description")
            VALUES
                (@ProgramId, @Type, @Name, @Date, @Location, @Responsible,
                 @ParticipatingProfessors, @ParticipatingStudents, @Description)
            RETURNING "Id"
            """;

        var parameters = new DynamicParameters(new
        {
            activity.ProgramId,
            Type = activity.Type.ToString(),
            activity.Name,
            activity.Date,
            activity.Location,
            activity.Responsible,
            ParticipatingProfessors = SerializeList(activity.ParticipatingProfessors),
            ParticipatingStudents = SerializeList(activity.ParticipatingStudents),
            activity.Description
        });

        var id = await databaseUtilities.ExecuteQuerySingle<int?>(query, parameters);
        if (id is null) return null;

        activity.Id = id.Value;
        return activity;
    }

    public async Task Update(Activity activity, CancellationToken cancellationToken = default)
    {
        const string command = """
            UPDATE activity
            SET "Name" = @Name,
                "Date" = @Date,
                "Location" = @Location,
                "Responsible" = @Responsible,
                "ParticipatingProfessors" = @ParticipatingProfessors,
                "ParticipatingStudents" = @ParticipatingStudents,
                "Description" = @Description
            WHERE "Id" = @Id
            """;

        await databaseUtilities.ExecuteCommandAsync(command, new
        {
            activity.Id,
            activity.Name,
            activity.Date,
            activity.Location,
            activity.Responsible,
            ParticipatingProfessors = SerializeList(activity.ParticipatingProfessors),
            ParticipatingStudents = SerializeList(activity.ParticipatingStudents),
            activity.Description
        });
    }

    public async Task Delete(Activity activity, CancellationToken cancellationToken = default)
    {
        const string command = """DELETE FROM activity WHERE "Id" = @Id""";
        await databaseUtilities.ExecuteCommandAsync(command, new { activity.Id });
    }

    public async Task<IReadOnlyList<Activity>> Get(
        int? programId, ActivityType? type, CancellationToken cancellationToken = default)
    {
        var conditions = new List<string>();
        var parameters = new DynamicParameters();

        if (programId is not null)
        {
            conditions.Add("\"ProgramId\" = @ProgramId");
            parameters.Add("ProgramId", programId);
        }
        if (type is not null)
        {
            conditions.Add("\"Type\" = @Type");
            parameters.Add("Type", type.ToString());
        }

        var whereClause = conditions.Count > 0 ? $"WHERE {string.Join(" AND ", conditions)}" : string.Empty;

        var rows = await databaseUtilities.ExecuteQuery<ActivityRow>(
            $"""
             SELECT {Columns}
             FROM activity
             {whereClause}
             ORDER BY "Date" DESC
             """, parameters);

        return rows.Select(r => r.ToActivity()).ToList();
    }

    private static string? SerializeList(List<string> values) =>
        values.Count == 0 ? null : JsonSerializer.Serialize(values);

    private static List<string> DeserializeList(string? json) =>
        string.IsNullOrWhiteSpace(json) ? [] : JsonSerializer.Deserialize<List<string>>(json) ?? [];
}
