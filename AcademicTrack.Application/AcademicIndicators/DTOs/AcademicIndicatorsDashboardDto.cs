namespace AcademicTrack.Application.AcademicIndicators.DTOs;

public class AcademicIndicatorsDashboardDto
{
    public ProgramInfoDto ProgramInfo { get; set; } = new();
    public StudentMetricsDto Students { get; set; } = new();
    public FacultyMetricsDto Faculty { get; set; } = new();
    public ResearchMetricsDto Research { get; set; } = new();
    public ExternalRelationsMetricsDto ExternalRelations { get; set; } = new();
    public GraduateMetricsDto Graduates { get; set; } = new();
}

public class ProgramInfoDto
{
    public string Name { get; set; } = string.Empty;
    public string SniesCode { get; set; } = string.Empty;
    public string Accreditation { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Modality { get; set; } = string.Empty;
    public int DurationSemesters { get; set; }
}

public class StudentMetricsDto
{
    public int TotalEnrolled { get; set; }
    public int NewStudents { get; set; }
    public string ApprovalRate { get; set; } = string.Empty;
    public string ReprobationRate { get; set; } = string.Empty;
    public string DropoutRate { get; set; } = string.Empty;
    public double AverageGraduationSemesters { get; set; }
    public int TargetGraduationSemesters { get; set; }
    public List<HistoricEnrolmentDto> HistoricEnrolment { get; set; } = new();
    public List<SemesterBreakdownDto> SemesterBreakdown { get; set; } = new();
}

public class HistoricEnrolmentDto
{
    public string Period { get; set; } = string.Empty;
    public int Inscritos { get; set; }
    public int Admitidos { get; set; }
    public int Matriculados { get; set; }
    public int Nuevos { get; set; }
    public int Graduados { get; set; }
    public int Retirados { get; set; }
    public double TasaDesercion { get; set; }
}

public class SemesterBreakdownDto
{
    public string Semestre { get; set; } = string.Empty;
    public int Estudiantes { get; set; }
    public int Aprobacion { get; set; }
    public int Reprobacion { get; set; }
    public int Desercion { get; set; }
}

public class FacultyMetricsDto
{
    public int Total { get; set; }
    public int FullTime { get; set; }
    public List<EducationLevelDto> EducationLevel { get; set; } = new();
    public List<HiringDistributionDto> HiringDistribution { get; set; } = new();
    public List<FeaturedFacultyDto> FeaturedFaculty { get; set; } = new();
}

public class EducationLevelDto
{
    public string Nivel { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public int Porcentaje { get; set; }
}

public class HiringDistributionDto
{
    public string Tipo { get; set; } = string.Empty;
    public int Cantidad { get; set; }
}

public class FeaturedFacultyDto
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Formacion { get; set; } = string.Empty;
    public string AreaConocimiento { get; set; } = string.Empty;
    public int ArticulosPublicados { get; set; }
    public string Vinculacion { get; set; } = string.Empty;
}

public class ResearchMetricsDto
{
    public int ScopusIndexed { get; set; }
    public int RecentPublications { get; set; }
    public int Innovations { get; set; }
    public int Patents { get; set; }
    public List<ResearchGroupDto> Groups { get; set; } = new();
    public List<HistoricPublicationDto> HistoricPublications { get; set; } = new();
    public List<FeaturedPublicationDto> FeaturedPublications { get; set; } = new();
}

public class ResearchGroupDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string Lider { get; set; } = string.Empty;
    public List<string> LineasInvestigacion { get; set; } = new();
    public int SemillerosActivos { get; set; }
}

public class HistoricPublicationDto
{
    public string Año { get; set; } = string.Empty;
    public int Scopus { get; set; }
    public int Nacionales { get; set; }
    public int Libros { get; set; }
}

public class FeaturedPublicationDto
{
    public string Titulo { get; set; } = string.Empty;
    public string Revista { get; set; } = string.Empty;
    public string Año { get; set; } = string.Empty;
    public string Doi { get; set; } = string.Empty;
}

public class ExternalRelationsMetricsDto
{
    public int NationalAgreements { get; set; }
    public int InternationalAgreements { get; set; }
    public List<ExtensionActivityDto> ExtensionActivities { get; set; } = new();
    public List<AgreementDto> AgreementsList { get; set; } = new();
}

public class ExtensionActivityDto
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public int Participantes { get; set; }
    public string Fecha { get; set; } = string.Empty;
    public string Impacto { get; set; } = string.Empty;
}

public class AgreementDto
{
    public string Institucion { get; set; } = string.Empty;
    public string Pais { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}

public class GraduateMetricsDto
{
    public string EmploymentRate { get; set; } = string.Empty;
    public double TimeToEmploymentMonths { get; set; }
    public double AverageIncomeSMLV { get; set; }
    public string EmployerSatisfaction { get; set; } = string.Empty;
    public List<PerformanceSectorDto> PerformanceSectors { get; set; } = new();
    public List<LocationDistributionDto> LocationDistribution { get; set; } = new();
}

public class PerformanceSectorDto
{
    public string Sector { get; set; } = string.Empty;
    public int Porcentaje { get; set; }
}

public class LocationDistributionDto
{
    public string Region { get; set; } = string.Empty;
    public int Porcentaje { get; set; }
}
