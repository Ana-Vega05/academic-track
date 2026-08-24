namespace AcademicTrack.Application.StudentAlumni.Cohortes.DTOs;

public class ComparacionCohorteDto
{
    public int PeriodoCohorteId { get; init; }

    public int Anio { get; init; }

    public string Semestre { get; init; } = string.Empty;

    public int Ingresaron { get; init; }

    public int Continuaron { get; init; }

    public int Desertores { get; init; }

    public int Graduados { get; init; }

    public decimal TasaDesercion { get; init; }

    public decimal TasaGraduacion { get; init; }
}