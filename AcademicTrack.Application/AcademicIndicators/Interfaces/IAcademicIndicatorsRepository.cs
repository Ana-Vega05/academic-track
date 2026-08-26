using AcademicTrack.Application.AcademicIndicators.DTOs;

namespace AcademicTrack.Application.AcademicIndicators.Interfaces;

public interface IAcademicIndicatorsRepository
{
    Task<AcademicIndicatorsDashboardDto> GetDashboardDataAsync(string? programName, string? period, CancellationToken cancellationToken = default);
    Task<UploadIndicatorResultDto> ProcessIndicatorUploadAsync(string indicatorType, string programName, string period, Stream fileStream, string fileName, long length, CancellationToken cancellationToken = default);
}
