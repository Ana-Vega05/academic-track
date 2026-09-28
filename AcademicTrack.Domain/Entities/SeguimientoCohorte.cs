namespace AcademicTrack.Domain.Entities;

public class SeguimientoCohorte
{
    public int Id { get; set; }

    public int ProgramaId { get; set; }

    public int PeriodoCohorteId { get; set; }

    public int SemestreSeguimiento { get; set; }

    public int Ingresaron { get; set; }

    public int Continuaron { get; set; }

    public int Cancelaciones { get; set; }

    public int Repitentes { get; set; }

    public int CambiosPrograma { get; set; }

    public int Desertores { get; set; }

    public int Graduados { get; set; }
}