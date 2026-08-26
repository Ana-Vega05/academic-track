using System.Collections.Concurrent;
using AcademicTrack.Application.AcademicIndicators.DTOs;
using AcademicTrack.Application.AcademicIndicators.Interfaces;
using AcademicTrack.Infrastructure.Persistence;
using AcademicTrack.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace AcademicTrack.Infrastructure.Repositories.AcademicIndicators;

public class AcademicIndicatorsRepository : IAcademicIndicatorsRepository
{
    private readonly AcademicTrackDbContext _dbContext;

    // Dynamic in-memory store holding metrics parsed directly from uploaded Excel files
    private static readonly ConcurrentDictionary<string, object> _inMemoryMetricsStore = new(StringComparer.OrdinalIgnoreCase);

    public AcademicIndicatorsRepository(AcademicTrackDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private static string NormalizeProgramName(string? program)
    {
        if (string.IsNullOrWhiteSpace(program) || program.Contains("Todos", StringComparison.OrdinalIgnoreCase))
        {
            return "Ingeniería de Sistemas";
        }
        return program.Trim();
    }

    public async Task<AcademicIndicatorsDashboardDto> GetDashboardDataAsync(string? program, string? period, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;

        var targetProgram = NormalizeProgramName(program);
        bool isSystems = targetProgram.Equals("Ingeniería de Sistemas", StringComparison.OrdinalIgnoreCase);

        string sniesCode = isSystems ? "12345" : "67890";
        int durationSemesters = 10;

        var academicKey = $"{targetProgram}_academic";
        var facultyKey = $"{targetProgram}_faculty";
        var researchKey = $"{targetProgram}_research";
        var externalKey = $"{targetProgram}_externalRelations";
        var graduatesKey = $"{targetProgram}_graduates";

        // Retrieve metrics dynamically parsed from uploaded Excel files
        StudentMetricsDto studentsMetrics;
        if (_inMemoryMetricsStore.TryGetValue(academicKey, out var parsedAcademicObj) && parsedAcademicObj is StudentMetricsDto parsedStudents)
        {
            var selectedPeriod = string.IsNullOrWhiteSpace(period)
                ? (parsedStudents.HistoricEnrolment.LastOrDefault()?.Period ?? "2025-1")
                : period;

            var matchingEnrolment = parsedStudents.HistoricEnrolment.FirstOrDefault(e => e.Period.Equals(selectedPeriod, StringComparison.OrdinalIgnoreCase))
                ?? parsedStudents.HistoricEnrolment.LastOrDefault();

            int totalEnrolled = matchingEnrolment?.Matriculados ?? parsedStudents.TotalEnrolled;
            int newStudents = matchingEnrolment?.Nuevos ?? parsedStudents.NewStudents;
            string dropoutRate = matchingEnrolment != null ? $"{matchingEnrolment.TasaDesercion}%" : parsedStudents.DropoutRate;

            studentsMetrics = new StudentMetricsDto
            {
                TotalEnrolled = totalEnrolled,
                NewStudents = newStudents,
                ApprovalRate = parsedStudents.ApprovalRate,
                ReprobationRate = parsedStudents.ReprobationRate,
                DropoutRate = dropoutRate,
                AverageGraduationSemesters = parsedStudents.AverageGraduationSemesters,
                TargetGraduationSemesters = parsedStudents.TargetGraduationSemesters,
                HistoricEnrolment = parsedStudents.HistoricEnrolment,
                SemesterBreakdown = new List<SemesterBreakdownDto>()
            };
        }
        else
        {
            studentsMetrics = new StudentMetricsDto
            {
                TotalEnrolled = 0,
                NewStudents = 0,
                ApprovalRate = "0%",
                ReprobationRate = "0%",
                DropoutRate = "0%",
                AverageGraduationSemesters = 0,
                TargetGraduationSemesters = 10,
                HistoricEnrolment = new List<HistoricEnrolmentDto>(),
                SemesterBreakdown = new List<SemesterBreakdownDto>()
            };
        }

        FacultyMetricsDto facultyMetrics;
        if (_inMemoryMetricsStore.TryGetValue(facultyKey, out var parsedFacultyObj) && parsedFacultyObj is FacultyMetricsDto parsedFaculty)
        {
            facultyMetrics = parsedFaculty;
        }
        else
        {
            facultyMetrics = new FacultyMetricsDto
            {
                Total = 0,
                FullTime = 0,
                EducationLevel = new List<EducationLevelDto>(),
                HiringDistribution = new List<HiringDistributionDto>(),
                FeaturedFaculty = new List<FeaturedFacultyDto>()
            };
        }

        ResearchMetricsDto researchMetrics;
        if (_inMemoryMetricsStore.TryGetValue(researchKey, out var parsedResearchObj) && parsedResearchObj is ResearchMetricsDto parsedResearch)
        {
            researchMetrics = parsedResearch;
        }
        else
        {
            researchMetrics = new ResearchMetricsDto
            {
                ScopusIndexed = 0,
                RecentPublications = 0,
                Innovations = 0,
                Patents = 0,
                Groups = new List<ResearchGroupDto>(),
                HistoricPublications = new List<HistoricPublicationDto>(),
                FeaturedPublications = new List<FeaturedPublicationDto>()
            };
        }

        ExternalRelationsMetricsDto externalMetrics;
        if (_inMemoryMetricsStore.TryGetValue(externalKey, out var parsedExternalObj) && parsedExternalObj is ExternalRelationsMetricsDto parsedExternal)
        {
            externalMetrics = parsedExternal;
        }
        else
        {
            externalMetrics = new ExternalRelationsMetricsDto
            {
                NationalAgreements = 0,
                InternationalAgreements = 0,
                ExtensionActivities = new List<ExtensionActivityDto>(),
                AgreementsList = new List<AgreementDto>()
            };
        }

        GraduateMetricsDto graduateMetrics;
        if (_inMemoryMetricsStore.TryGetValue(graduatesKey, out var parsedGraduatesObj) && parsedGraduatesObj is GraduateMetricsDto parsedGraduates)
        {
            graduateMetrics = parsedGraduates;
        }
        else
        {
            graduateMetrics = new GraduateMetricsDto
            {
                EmploymentRate = "0%",
                TimeToEmploymentMonths = 0,
                AverageIncomeSMLV = 0,
                EmployerSatisfaction = "0%",
                PerformanceSectors = new List<PerformanceSectorDto>(),
                LocationDistribution = new List<LocationDistributionDto>()
            };
        }

        var dashboard = new AcademicIndicatorsDashboardDto
        {
            ProgramInfo = new ProgramInfoDto
            {
                Name = targetProgram,
                SniesCode = sniesCode,
                Accreditation = "Alta Calidad (Res. MinEducación 01425)",
                Director = isSystems ? "Ing. Alvaro Oñate" : "Ing. María Fernanda Gómez",
                Modality = "Presencial",
                DurationSemesters = durationSemesters
            },
            Students = studentsMetrics,
            Faculty = facultyMetrics,
            Research = researchMetrics,
            ExternalRelations = externalMetrics,
            Graduates = graduateMetrics
        };

        return dashboard;
    }

    public async Task<UploadIndicatorResultDto> ProcessIndicatorUploadAsync(string indicatorType, string programName, string period, Stream fileStream, string fileName, long length, CancellationToken cancellationToken = default)
    {
        await Task.Delay(100, cancellationToken);

        var targetProgram = NormalizeProgramName(programName);
        string sectionKey = $"{targetProgram}_academic";

        if (indicatorType.Contains("Docente", StringComparison.OrdinalIgnoreCase) || indicatorType.Contains("Formación", StringComparison.OrdinalIgnoreCase) || indicatorType.Equals("faculty", StringComparison.OrdinalIgnoreCase))
        {
            sectionKey = $"{targetProgram}_faculty";
            var facultyMetrics = ExcelParser.ParseFacultyMetrics(fileStream);
            _inMemoryMetricsStore[sectionKey] = facultyMetrics;
        }
        else if (indicatorType.Contains("Investigación", StringComparison.OrdinalIgnoreCase) || indicatorType.Contains("Innovación", StringComparison.OrdinalIgnoreCase) || indicatorType.Equals("research", StringComparison.OrdinalIgnoreCase))
        {
            sectionKey = $"{targetProgram}_research";
            var researchMetrics = ExcelParser.ParseResearchMetrics(fileStream);
            _inMemoryMetricsStore[sectionKey] = researchMetrics;
        }
        else if (indicatorType.Contains("Relaciones", StringComparison.OrdinalIgnoreCase) || indicatorType.Contains("Convenios", StringComparison.OrdinalIgnoreCase) || indicatorType.Contains("Proyección", StringComparison.OrdinalIgnoreCase) || indicatorType.Equals("externalRelations", StringComparison.OrdinalIgnoreCase))
        {
            sectionKey = $"{targetProgram}_externalRelations";
            var externalMetrics = ExcelParser.ParseExternalRelationsMetrics(fileStream);
            _inMemoryMetricsStore[sectionKey] = externalMetrics;
        }
        else if (indicatorType.Contains("Egresados", StringComparison.OrdinalIgnoreCase) || indicatorType.Equals("graduates", StringComparison.OrdinalIgnoreCase))
        {
            sectionKey = $"{targetProgram}_graduates";
            var graduateMetrics = ExcelParser.ParseGraduateMetrics(fileStream);
            _inMemoryMetricsStore[sectionKey] = graduateMetrics;
        }
        else
        {
            // Academic / Students (e.g. "2. Cuadro SACES")
            var studentMetrics = ExcelParser.ParseStudentMetrics(fileStream);
            _inMemoryMetricsStore[sectionKey] = studentMetrics;
        }

        int estimatedRecords = (int)(length / 120);
        if (estimatedRecords < 1) estimatedRecords = 1;

        return new UploadIndicatorResultDto
        {
            Success = true,
            Message = $"El archivo Excel '{fileName}' fue leído y procesado exitosamente por el analizador dinámico para {targetProgram}.",
            IndicatorType = indicatorType,
            FileName = fileName,
            FileSizeBytes = length,
            ProcessedAt = DateTime.UtcNow,
            ProcessedRecords = estimatedRecords
        };
    }
}
