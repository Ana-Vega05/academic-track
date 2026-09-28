namespace AcademicTrack.Domain.Models.Requests;

public class UpdateActivityRequest
{
    public string Name { get; init; } = string.Empty;
    public DateOnly Date { get; init; }
    public string? Location { get; init; }
    public string Responsible { get; init; } = string.Empty;
    public List<string> ParticipatingProfessors { get; init; } = [];
    public List<string> ParticipatingStudents { get; init; } = [];
    public string? Description { get; init; }
}