namespace AcademicTrack.Application.StudentAlumni.Egresados.DTOs;

public class SeguimientoEgresadoDto
{
    public int ProgramaId { get; init; }

    public short AnioGraduacion { get; init; }

    public int TotalEgresados { get; init; }

    public int Empleados { get; init; }

    public int EmpleadosRelacionadosCarrera { get; init; }

    public int EmpleadosNoRelacionadosCarrera { get; init; }

    public decimal? TiempoPromedioConseguirEmpleoMeses { get; init; }

    public int ContratoIndefinido { get; init; }

    public int ContratoTerminoFijo { get; init; }

    public int ContratoPrestacionServicios { get; init; }

    public int ContratoOtro { get; init; }

    public int ContinuanEstudios { get; init; }

    public decimal TasaEmpleabilidad { get; init; }

    public decimal TasaRelacionCarrera { get; init; }

    public decimal TasaContinuanEstudios { get; init; }
}