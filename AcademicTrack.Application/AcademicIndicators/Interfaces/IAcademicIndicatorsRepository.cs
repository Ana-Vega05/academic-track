using AcademicTrack.Application.AcademicIndicators.DTOs;

namespace AcademicTrack.Application.AcademicIndicators.Interfaces;

public interface IAcademicIndicatorsRepository
{
    Task<AcademicIndicatorsDashboardDto> GetDashboardDataAsync(string? programName, string? period, CancellationToken cancellationToken = default);
}
