namespace AcademicTrack.Application.StudentAlumni.Cohortes.DTOs;

public class SeguimientoCohorteDto
{
    public int ProgramaId { get; init; }

    public int PeriodoCohorteId { get; init; }

    public int SemestreSeguimiento { get; init; }

    public int Ingresaron { get; init; }

    public int Continuaron { get; init; }

    public int Cancelaciones { get; init; }

    public int Repitentes { get; init; }

    public int CambiosPrograma { get; init; }

    public int Desertores { get; init; }

    public int Graduados { get; init; }
}