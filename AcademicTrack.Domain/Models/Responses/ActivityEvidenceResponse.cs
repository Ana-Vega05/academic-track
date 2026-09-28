namespace AcademicTrack.Domain.Models.Responses;

public class ActivityEvidenceResponse
{
    public string Url { get; init; } = string.Empty;
    public DateOnly UploadDate { get; init; }
}
