using AcademicTrack.Application.AcademicIndicators.DTOs;
using AcademicTrack.Application.AcademicIndicators.Interfaces;
using AcademicTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AcademicTrack.Infrastructure.Repositories.AcademicIndicators;

public class AcademicIndicatorsRepository : IAcademicIndicatorsRepository
{
    private readonly AcademicTrackDbContext _dbContext;

    public AcademicIndicatorsRepository(AcademicTrackDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AcademicIndicatorsDashboardDto> GetDashboardDataAsync(string? programName, string? period, CancellationToken cancellationToken = default)
    {
        var targetProgram = string.IsNullOrWhiteSpace(programName) || programName == "Todos los Programas"
            ? "Ingeniería de Sistemas"
            : programName;

        var targetPeriod = string.IsNullOrWhiteSpace(period)
            ? "2025-1"
            : period;

        var programDb = await _dbContext.Programas
            .FirstOrDefaultAsync(p => p.Nombre == targetProgram, cancellationToken);

        var isSystems = targetProgram.Contains("Sistemas", StringComparison.OrdinalIgnoreCase);

        var dashboard = new AcademicIndicatorsDashboardDto
        {
            ProgramInfo = new ProgramInfoDto
            {
                Name = targetProgram,
                SniesCode = !string.IsNullOrEmpty(programDb?.CodigoSnies) ? programDb.CodigoSnies : (isSystems ? "12345" : "67890"),
                Accreditation = "Alta Calidad (Res. MinEducación 01425)",
                Director = isSystems ? "Ing. Alvaro Oñate" : "Ing. María Fernanda Gómez",
                Modality = "Presencial",
                DurationSemesters = 10
            },
            Students = new StudentMetricsDto
            {
                TotalEnrolled = isSystems ? 650 : 1240,
                NewStudents = isSystems ? 85 : 190,
                ApprovalRate = "86.4%",
                ReprobationRate = "9.2%",
                DropoutRate = "4.4%",
                AverageGraduationSemesters = 10.8,
                TargetGraduationSemesters = 10,
                HistoricEnrolment = new List<HistoricEnrolmentDto>
                {
                    new() { Period = "2023-1", Matriculados = 580, Nuevos = 75 },
                    new() { Period = "2023-2", Matriculados = 610, Nuevos = 80 },
                    new() { Period = "2024-1", Matriculados = 630, Nuevos = 82 },
                    new() { Period = "2024-2", Matriculados = 645, Nuevos = 84 },
                    new() { Period = targetPeriod, Matriculados = isSystems ? 650 : 1240, Nuevos = isSystems ? 85 : 190 }
                },
                SemesterBreakdown = new List<SemesterBreakdownDto>
                {
                    new() { Semestre = "Sem 1-2", Estudiantes = 170, Aprobacion = 78, Reprobacion = 16, Desercion = 6 },
                    new() { Semestre = "Sem 3-4", Estudiantes = 150, Aprobacion = 84, Reprobacion = 11, Desercion = 5 },
                    new() { Semestre = "Sem 5-6", Estudiantes = 140, Aprobacion = 89, Reprobacion = 8, Desercion = 3 },
                    new() { Semestre = "Sem 7-8", Estudiantes = 105, Aprobacion = 93, Reprobacion = 5, Desercion = 2 },
                    new() { Semestre = "Sem 9-10", Estudiantes = 85, Aprobacion = 97, Reprobacion = 2, Desercion = 1 }
                }
            },
            Faculty = new FacultyMetricsDto
            {
                Total = isSystems ? 32 : 78,
                FullTime = isSystems ? 20 : 48,
                EducationLevel = new List<EducationLevelDto>
                {
                    new() { Nivel = "Doctorado", Cantidad = 8, Porcentaje = 25 },
                    new() { Nivel = "Maestría", Cantidad = 18, Porcentaje = 56 },
                    new() { Nivel = "Especialización", Cantidad = 4, Porcentaje = 13 },
                    new() { Nivel = "Pregrado", Cantidad = 2, Porcentaje = 6 }
                },
                HiringDistribution = new List<HiringDistributionDto>
                {
                    new() { Tipo = "Tiempo Completo", Cantidad = 20 },
                    new() { Tipo = "Medio Tiempo", Cantidad = 5 },
                    new() { Tipo = "Cátedra", Cantidad = 7 }
                },
                FeaturedFaculty = new List<FeaturedFacultyDto>
                {
                    new() { Id = "1", Nombre = "Dr. Carlos Alberto Mendoza", Formacion = "Ph.D. en Ciencias de la Computación", AreaConocimiento = "Inteligencia Artificial y ML", ArticulosPublicados = 14, Vinculacion = "Planta" },
                    new() { Id = "2", Nombre = "Dra. Elena Patricia Restrepo", Formacion = "Ph.D. en Ingeniería de Software", AreaConocimiento = "Arquitectura de Software y Cloud", ArticulosPublicados = 11, Vinculacion = "Planta" },
                    new() { Id = "3", Nombre = "MSc. Roberto José Silva", Formacion = "Magíster en Ciberseguridad", AreaConocimiento = "Seguridad Informática y Redes", ArticulosPublicados = 6, Vinculacion = "Ocasional" }
                }
            },
            Research = new ResearchMetricsDto
            {
                ScopusIndexed = isSystems ? 28 : 54,
                RecentPublications = isSystems ? 12 : 26,
                Innovations = 7,
                Patents = 2,
                Groups = new List<ResearchGroupDto>
                {
                    new() { Nombre = "GISICO", Categoria = "A", Lider = "Dr. Carlos Mendoza", LineasInvestigacion = new List<string> { "IA Aplicada", "Visión por Computador", "Big Data" }, SemillerosActivos = 4 },
                    new() { Nombre = "AITICE", Categoria = "B", Lider = "Dra. Elena Restrepo", LineasInvestigacion = new List<string> { "Ingeniería de Software", "IoT", "Ciberseguridad" }, SemillerosActivos = 3 }
                },
                HistoricPublications = new List<HistoricPublicationDto>
                {
                    new() { Año = "2021", Scopus = 4, Nacionales = 8, Libros = 2 },
                    new() { Año = "2022", Scopus = 6, Nacionales = 9, Libros = 1 },
                    new() { Año = "2023", Scopus = 9, Nacionales = 11, Libros = 3 },
                    new() { Año = "2024", Scopus = 12, Nacionales = 14, Libros = 2 }
                },
                FeaturedPublications = new List<FeaturedPublicationDto>
                {
                    new() { Titulo = "Deep Learning Models for Agricultural Crop Classification in Caribbean Region", Revista = "Computers and Electronics in Agriculture (Elsevier Q1)", Año = "2024", Doi = "10.1016/j.compag.2024.108920" },
                    new() { Titulo = "Microservices Architecture Performance in Distributed Educational Platforms", Revista = "IEEE Access (Q1)", Año = "2023", Doi = "10.1109/ACCESS.2023.3289100" }
                }
            },
            ExternalRelations = new ExternalRelationsMetricsDto
            {
                NationalAgreements = 14,
                InternationalAgreements = 6,
                ExtensionActivities = new List<ExtensionActivityDto>
                {
                    new() { Id = "1", Nombre = "Capacitación en Alfabetización Digital a Comunidades Rurales del Cesar", Tipo = "Proyección Social", Participantes = 240, Fecha = "2024-2", Impacto = "Alto Impacto Regional" },
                    new() { Id = "2", Nombre = "Desarrollo de Software de Gestión para Mypimes de Valledupar", Tipo = "Extensión Tecnológica", Participantes = 35, Fecha = "2024-1", Impacto = "Fortalecimiento Empresarial" }
                },
                AgreementsList = new List<AgreementDto>
                {
                    new() { Institucion = "Universidad Politécnica de Valencia", Pais = "España", Tipo = "Movilidad e Investigación", Estado = "Vigente" },
                    new() { Institucion = "Ecopetrol S.A.", Pais = "Colombia", Tipo = "Prácticas Empresariales", Estado = "Vigente" },
                    new() { Institucion = "Tecnológico de Monterrey", Pais = "México", Tipo = "Intercambio Académico", Estado = "Vigente" }
                }
            },
            Graduates = new GraduateMetricsDto
            {
                EmploymentRate = "88.5%",
                TimeToEmploymentMonths = 4.2,
                AverageIncomeSMLV = 3.4,
                EmployerSatisfaction = "94.2%",
                PerformanceSectors = new List<PerformanceSectorDto>
                {
                    new() { Sector = "Tecnología & Software", Porcentaje = 45 },
                    new() { Sector = "Sector Financiero & Banca", Porcentaje = 22 },
                    new() { Sector = "Telecomunicaciones", Porcentaje = 15 },
                    new() { Sector = "Gobierno & Sector Público", Porcentaje = 10 },
                    new() { Sector = "Consultoría Independiente", Porcentaje = 8 }
                },
                LocationDistribution = new List<LocationDistributionDto>
                {
                    new() { Region = "Local (Valledupar / Cesar)", Porcentaje = 40 },
                    new() { Region = "Nacional (Bogotá, Mde, Bq)", Porcentaje = 42 },
                    new() { Region = "Internacional / Remoto USA-EUR", Porcentaje = 18 }
                }
            }
        };

        return dashboard;
    }
}
