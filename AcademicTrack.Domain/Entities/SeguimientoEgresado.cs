namespace AcademicTrack.Domain.Entities;

public class SeguimientoEgresado
{
    public int Id { get; set; }

    public int ProgramaId { get; set; }

    public short AnioGraduacion { get; set; }

    public int TotalEgresados { get; set; }

    public int Empleados { get; set; }

    public int EmpleadosRelacionadosCarrera { get; set; }

    public int EmpleadosNoRelacionadosCarrera { get; set; }

    public decimal? TiempoPromedioConseguirEmpleoMeses { get; set; }

    public int ContratoIndefinido { get; set; }

    public int ContratoTerminoFijo { get; set; }

    public int ContratoPrestacionServicios { get; set; }

    public int ContratoOtro { get; set; }

    public int ContinuanEstudios { get; set; }
}