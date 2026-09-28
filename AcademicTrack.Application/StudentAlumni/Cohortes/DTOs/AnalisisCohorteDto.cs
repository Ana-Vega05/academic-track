namespace AcademicTrack.Application.StudentAlumni.Cohortes.DTOs;

public class AnalisisCohorteDto
{
    public int ProgramaId { get; init; }

    public int PeriodoCohorteId { get; init; }

    public int TotalIngresaron { get; init; }

    public int TotalContinuaron { get; init; }

    public int TotalCancelaciones { get; init; }

    public int TotalRepitentes { get; init; }

    public int TotalCambiosPrograma { get; init; }

    public int TotalDesertores { get; init; }

    public int TotalGraduados { get; init; }

    public decimal TasaDesercion { get; init; }

    public decimal TasaGraduacion { get; init; }

    public int SemestreMayorDesercion { get; init; }

    public int MayorDesercion { get; init; }
}