using AcademicTrack.Application.StudentAlumni.Egresados.DTOs;
using AcademicTrack.Application.StudentAlumni.Egresados.Interfaces;

namespace AcademicTrack.Application.StudentAlumni.Egresados.Services;

public class SeguimientoEgresadoService
{
    private readonly ISeguimientoEgresadoRepository _repository;

    public SeguimientoEgresadoService(
        ISeguimientoEgresadoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<SeguimientoEgresadoDto>> ObtenerPorProgramaAsync(
        int programaId,
        CancellationToken cancellationToken = default)
    {
        ValidarPrograma(programaId);

        var registros = await _repository.ObtenerPorProgramaAsync(
            programaId,
            cancellationToken);

        return registros
            .Select(MapearSeguimiento)
            .ToList();
    }

    public async Task<AnalisisEgresadoDto?> AnalizarAsync(
        int programaId,
        short anioGraduacion,
        CancellationToken cancellationToken = default)
    {
        ValidarPrograma(programaId);

        if (anioGraduacion <= 0)
        {
            throw new ArgumentException(
                "El año de graduación debe ser válido.",
                nameof(anioGraduacion));
        }

        var registro = await _repository.ObtenerPorProgramaYAnioAsync(
            programaId,
            anioGraduacion,
            cancellationToken);

        if (registro is null)
        {
            return null;
        }

        var distribuciones = await _repository.ObtenerDistribucionesAsync(
            registro.Id,
            cancellationToken);

        var tasaEmpleabilidad = CalcularPorcentaje(
            registro.Empleados,
            registro.TotalEgresados);

        var tasaRelacionCarrera = CalcularPorcentaje(
            registro.EmpleadosRelacionadosCarrera,
            registro.Empleados);

        var tasaContinuanEstudios = CalcularPorcentaje(
            registro.ContinuanEstudios,
            registro.TotalEgresados);

        return new AnalisisEgresadoDto
        {
            ProgramaId = registro.ProgramaId,
            AnioGraduacion = registro.AnioGraduacion,
            TotalEgresados = registro.TotalEgresados,
            Empleados = registro.Empleados,
            EmpleadosRelacionadosCarrera =
                registro.EmpleadosRelacionadosCarrera,
            EmpleadosNoRelacionadosCarrera =
                registro.EmpleadosNoRelacionadosCarrera,
            TiempoPromedioConseguirEmpleoMeses =
                registro.TiempoPromedioConseguirEmpleoMeses,
            ContratoIndefinido = registro.ContratoIndefinido,
            ContratoTerminoFijo = registro.ContratoTerminoFijo,
            ContratoPrestacionServicios =
                registro.ContratoPrestacionServicios,
            ContratoOtro = registro.ContratoOtro,
            ContinuanEstudios = registro.ContinuanEstudios,

            TasaEmpleabilidad = tasaEmpleabilidad,
            TasaRelacionCarrera = tasaRelacionCarrera,
            TasaContinuanEstudios = tasaContinuanEstudios,

            Distribuciones = distribuciones
                .Select(x => new DistribucionEgresadoDto
                {
                    Tipo = x.Tipo,
                    Categoria = x.Categoria,
                    Cantidad = x.Cantidad
                })
                .ToList()
        };
    }

    public async Task<IReadOnlyList<DistribucionEgresadoDto>> ObtenerDistribucionesAsync(
        int seguimientoEgresadoId,
        CancellationToken cancellationToken = default)
    {
        if (seguimientoEgresadoId <= 0)
        {
            throw new ArgumentException(
                "El seguimiento de egresado debe ser válido.",
                nameof(seguimientoEgresadoId));
        }

        var registros = await _repository.ObtenerDistribucionesAsync(
            seguimientoEgresadoId,
            cancellationToken);

        return registros
            .Select(x => new DistribucionEgresadoDto
            {
                Tipo = x.Tipo,
                Categoria = x.Categoria,
                Cantidad = x.Cantidad
            })
            .ToList();
    }

    private static SeguimientoEgresadoDto MapearSeguimiento(
        Domain.Entities.SeguimientoEgresado registro)
    {
        return new SeguimientoEgresadoDto
        {
            ProgramaId = registro.ProgramaId,
            AnioGraduacion = registro.AnioGraduacion,
            TotalEgresados = registro.TotalEgresados,
            Empleados = registro.Empleados,
            EmpleadosRelacionadosCarrera =
                registro.EmpleadosRelacionadosCarrera,
            EmpleadosNoRelacionadosCarrera =
                registro.EmpleadosNoRelacionadosCarrera,
            TiempoPromedioConseguirEmpleoMeses =
                registro.TiempoPromedioConseguirEmpleoMeses,
            ContratoIndefinido = registro.ContratoIndefinido,
            ContratoTerminoFijo = registro.ContratoTerminoFijo,
            ContratoPrestacionServicios =
                registro.ContratoPrestacionServicios,
            ContratoOtro = registro.ContratoOtro,
            ContinuanEstudios = registro.ContinuanEstudios,

            TasaEmpleabilidad = CalcularPorcentaje(
                registro.Empleados,
                registro.TotalEgresados),

            TasaRelacionCarrera = CalcularPorcentaje(
                registro.EmpleadosRelacionadosCarrera,
                registro.Empleados),

            TasaContinuanEstudios = CalcularPorcentaje(
                registro.ContinuanEstudios,
                registro.TotalEgresados)
        };
    }

    private static decimal CalcularPorcentaje(
        int cantidad,
        int total)
    {
        return total > 0
            ? Math.Round(
                (decimal)cantidad / total * 100,
                2)
            : 0;
    }

    private static void ValidarPrograma(int programaId)
    {
        if (programaId <= 0)
        {
            throw new ArgumentException(
                "El programa debe ser válido.",
                nameof(programaId));
        }
    }
}