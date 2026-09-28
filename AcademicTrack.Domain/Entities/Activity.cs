using AcademicTrack.Domain.Enums;

namespace AcademicTrack.Domain.Entities;

public class Activity
{
    public int Id { get; set; }
    public int ProgramId { get; set; }
    public ActivityType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public string? Location { get; set; }
    public string Responsible { get; set; } = string.Empty;
    public List<string> ParticipatingProfessors { get; set; } = [];
    public List<string> ParticipatingStudents { get; set; } = [];
    public string? Description { get; set; }
}
