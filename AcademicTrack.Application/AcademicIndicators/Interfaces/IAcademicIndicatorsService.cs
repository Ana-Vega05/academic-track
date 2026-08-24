using AcademicTrack.Application.AcademicIndicators.DTOs;

namespace AcademicTrack.Application.AcademicIndicators.Interfaces;

public interface IAcademicIndicatorsService
{
    Task<AcademicIndicatorsDashboardDto> GetDashboardDataAsync(string? programName, string? period, CancellationToken cancellationToken = default);
}
