namespace AcademicTrack.Application.AcademicIndicators.DTOs;

public class UploadIndicatorResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string IndicatorType { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
    public int ProcessedRecords { get; set; }
}
