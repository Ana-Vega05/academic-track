using AcademicTrack.Application.StudentAlumni.Cohortes.DTOs;
using AcademicTrack.Application.StudentAlumni.Cohortes.Interfaces;

namespace AcademicTrack.Application.StudentAlumni.Cohortes.Services;

public class SeguimientoCohorteService : ISeguimientoCohorteService
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
    int periodoCohorteId,
    CancellationToken cancellationToken = default)
{
    var registros = await _repository.ObtenerPorCohorteAsync(
    programaId,
    periodoCohorteId,
    cancellationToken);

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


public async Task<IReadOnlyList<ComparacionCohorteDto>> CompararCohortesAsync(
    int programaId,
    CancellationToken cancellationToken = default)
{
    var registros = await _repository.ObtenerComparacionAsync(
    programaId,
    cancellationToken);
    return registros
        .GroupBy(x => new
        {
            x.PeriodoCohorteId,
            x.Anio,
            x.Semestre
        })
        .Select(grupo =>
        {
            var registrosCohorte = grupo
                .OrderBy(x => x.SemestreSeguimiento)
                .ToList();

            var primerRegistro = registrosCohorte.First();
            var ultimoRegistro = registrosCohorte.Last();

            var ingresaron = primerRegistro.Ingresaron;
            var continuaron = ultimoRegistro.Continuaron;
            var desertores = registrosCohorte.Sum(x => x.Desertores);
            var graduados = ultimoRegistro.Graduados;

            return new ComparacionCohorteDto
            {
                PeriodoCohorteId = grupo.Key.PeriodoCohorteId,
                Anio = grupo.Key.Anio,
                Semestre = grupo.Key.Semestre,

                Ingresaron = ingresaron,
                Continuaron = continuaron,
                Desertores = desertores,
                Graduados = graduados,

                TasaDesercion = ingresaron > 0
                    ? Math.Round(
                        (decimal)desertores / ingresaron * 100,
                        2)
                    : 0,

                TasaGraduacion = ingresaron > 0
                    ? Math.Round(
                        (decimal)graduados / ingresaron * 100,
                        2)
                    : 0
            };
        })
        .ToList();
}




}