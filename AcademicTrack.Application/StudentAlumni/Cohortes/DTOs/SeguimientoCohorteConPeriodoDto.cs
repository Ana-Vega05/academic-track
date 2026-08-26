namespace AcademicTrack.Application.StudentAlumni.Cohortes.DTOs;

public class SeguimientoCohorteConPeriodoDto
{
    public int PeriodoCohorteId { get; init; }

    public short Anio { get; init; }

    public string Semestre { get; init; } = string.Empty;

    public int SemestreSeguimiento { get; init; }

    public int Ingresaron { get; init; }

    public int Continuaron { get; init; }

    public int Desertores { get; init; }

    public int Graduados { get; init; }
}