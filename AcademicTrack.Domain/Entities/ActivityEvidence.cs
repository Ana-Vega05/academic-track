namespace AcademicTrack.Domain.Entities;

public class ActivityEvidence
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public string Url { get; set; } = string.Empty;
    public DateOnly UploadDate { get; set; }
}
