using AcademicTrack.Application.AcademicIndicators.DTOs;
using AcademicTrack.Application.AcademicIndicators.Interfaces;

namespace AcademicTrack.Application.AcademicIndicators.Services;

public class AcademicIndicatorsService : IAcademicIndicatorsService
{
    private readonly IAcademicIndicatorsRepository _repository;

    public AcademicIndicatorsService(IAcademicIndicatorsRepository repository)
    {
        _repository = repository;
    }

    public async Task<AcademicIndicatorsDashboardDto> GetDashboardDataAsync(string? programName, string? period, CancellationToken cancellationToken = default)
    {
        return await _repository.GetDashboardDataAsync(programName, period, cancellationToken);
    }
}
