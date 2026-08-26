using AcademicTrack.Domain.Enums;

namespace AcademicTrack.Domain.Models.Responses;

public class ActivityResponse
{
    public int Id { get; init; }
    public int ProgramId { get; init; }
    public ActivityType Type { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateOnly Date { get; init; }
    public string? Location { get; init; }
    public string Responsible { get; init; } = string.Empty;
    public IReadOnlyList<string> ParticipatingProfessors { get; init; } = [];
    public IReadOnlyList<string> ParticipatingStudents { get; init; } = [];
    public string? Description { get; init; }
    public IReadOnlyList<ActivityEvidenceResponse> Evidences { get; init; } = [];
}
