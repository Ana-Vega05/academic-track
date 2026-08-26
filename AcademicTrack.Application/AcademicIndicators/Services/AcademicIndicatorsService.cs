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

    public async Task<UploadIndicatorResultDto> UploadIndicatorFileAsync(string indicatorType, string programName, string period, Stream fileStream, string fileName, long length, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(indicatorType))
        {
            throw new ArgumentException("Debe especificar el tipo de indicador a procesar.");
        }

        if (fileStream == null || length == 0)
        {
            throw new ArgumentException("El archivo adjunto está vacío o es inválido.");
        }

        return await _repository.ProcessIndicatorUploadAsync(indicatorType, programName, period, fileStream, fileName, length, cancellationToken);
    }
}
