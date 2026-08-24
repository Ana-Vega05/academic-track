using AcademicTrack.Application.StudentAlumni.Cohortes.DTOs;
using AcademicTrack.Application.StudentAlumni.Cohortes.Interfaces;

namespace AcademicTrack.Application.StudentAlumni.Cohortes.Services;

public class SeguimientoCohorteService
{
    private readonly ISeguimientoCohorteRepository _repository;

    public SeguimientoCohorteService(
        ISeguimientoCohorteRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<SeguimientoCohorteDto>> ObtenerPorCohorteAsync(
        int programaId,
        int periodoCohorteId,
        CancellationToken cancellationToken = default)
    {
        ValidarParametros(programaId, periodoCohorteId);

        var registros = await _repository.ObtenerPorCohorteAsync(
            programaId,
            periodoCohorteId,
            cancellationToken);

        return registros
            .Select(registro => new SeguimientoCohorteDto
            {
                ProgramaId = registro.ProgramaId,
                PeriodoCohorteId = registro.PeriodoCohorteId,
                SemestreSeguimiento = registro.SemestreSeguimiento,
                Ingresaron = registro.Ingresaron,
                Continuaron = registro.Continuaron,
                Cancelaciones = registro.Cancelaciones,
                Repitentes = registro.Repitentes,
                CambiosPrograma = registro.CambiosPrograma,
                Desertores = registro.Desertores,
                Graduados = registro.Graduados
            })
            .ToList();
    }

    private static void ValidarParametros(
        int programaId,
        int periodoCohorteId)
    {
        if (programaId <= 0)
        {
            throw new ArgumentException(
                "El programa debe ser válido.",
                nameof(programaId));
        }

        if (periodoCohorteId <= 0)
        {
            throw new ArgumentException(
                "El periodo de cohorte debe ser válido.",
                nameof(periodoCohorteId));
        }
    }


    public async Task<AnalisisCohorteDto?> AnalizarCohorteAsync(
    int programaId,
    int periodoCohorteId)
{
    var registros = await _repository.ObtenerPorCohorteAsync(
        programaId,
        periodoCohorteId);

    if (registros.Count == 0)
    {
        return null;
    }

    var primerRegistro = registros
        .OrderBy(x => x.SemestreSeguimiento)
        .First();

    var totalIngresaron = primerRegistro.Ingresaron;

    var totalContinuaron = registros
        .OrderByDescending(x => x.SemestreSeguimiento)
        .Select(x => x.Continuaron)
        .First();

    var totalCancelaciones = registros.Sum(x => x.Cancelaciones);

    var totalRepitentes = registros.Sum(x => x.Repitentes);

    var totalCambiosPrograma = registros.Sum(x => x.CambiosPrograma);

    var totalDesertores = registros.Sum(x => x.Desertores);

    var totalGraduados = registros
        .OrderByDescending(x => x.SemestreSeguimiento)
        .Select(x => x.Graduados)
        .First();

    var tasaDesercion = totalIngresaron > 0
        ? Math.Round((decimal)totalDesertores / totalIngresaron * 100, 2)
        : 0;

    var tasaGraduacion = totalIngresaron > 0
        ? Math.Round((decimal)totalGraduados / totalIngresaron * 100, 2)
        : 0;

    var semestreMayorDesercion = registros
        .OrderByDescending(x => x.Desertores)
        .First();

    return new AnalisisCohorteDto
    {
        ProgramaId = programaId,
        PeriodoCohorteId = periodoCohorteId,
        TotalIngresaron = totalIngresaron,
        TotalContinuaron = totalContinuaron,
        TotalCancelaciones = totalCancelaciones,
        TotalRepitentes = totalRepitentes,
        TotalCambiosPrograma = totalCambiosPrograma,
        TotalDesertores = totalDesertores,
        TotalGraduados = totalGraduados,
        TasaDesercion = tasaDesercion,
        TasaGraduacion = tasaGraduacion,
        SemestreMayorDesercion =
            semestreMayorDesercion.SemestreSeguimiento,
        MayorDesercion =
            semestreMayorDesercion.Desertores
    };
}

}