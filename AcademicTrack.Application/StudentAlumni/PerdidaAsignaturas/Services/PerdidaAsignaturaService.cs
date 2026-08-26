using AcademicTrack.Application.StudentAlumni.PerdidaAsignaturas.DTOs;
using AcademicTrack.Application.StudentAlumni.PerdidaAsignaturas.Interfaces;

namespace AcademicTrack.Application.StudentAlumni.PerdidaAsignaturas.Services;

public class PerdidaAsignaturaService
{
    private readonly IPerdidaAsignaturaRepository _repository;

    public PerdidaAsignaturaService(
        IPerdidaAsignaturaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<PerdidaAsignaturaDto>> ObtenerPorPeriodoAsync(
        int programaId,
        int periodoId,
        CancellationToken cancellationToken = default)
    {
        ValidarParametros(programaId, periodoId);

        var registros = await _repository.ObtenerPorPeriodoAsync(
            programaId,
            periodoId,
            cancellationToken);

        return registros
            .Select(registro => new PerdidaAsignaturaDto
            {
                Id = registro.Id,
                ProgramaId = registro.ProgramaId,
                PeriodoId = registro.PeriodoId,
                Asignatura = registro.Asignatura,
                Matriculados = registro.Matriculados,
                Aprobados = registro.Aprobados,
                Reprobados = registro.Reprobados,
                PorcentajePerdida = registro.PorcentajePerdida
            })
            .ToList();
    }

    public async Task<AnalisisPerdidaAsignaturaDto?> AnalizarPeriodoAsync(
        int programaId,
        int periodoId,
        CancellationToken cancellationToken = default)
    {
        ValidarParametros(programaId, periodoId);

        var registros = await _repository.ObtenerPorPeriodoAsync(
            programaId,
            periodoId,
            cancellationToken);

        if (registros.Count == 0)
        {
            return null;
        }

        var totalMatriculados = registros.Sum(x => x.Matriculados);
        var totalAprobados = registros.Sum(x => x.Aprobados);
        var totalReprobados = registros.Sum(x => x.Reprobados);

        var porcentajePerdidaGeneral = totalMatriculados > 0
            ? Math.Round(
                (decimal)totalReprobados / totalMatriculados * 100,
                2)
            : 0;

        var mayorPerdida = registros
            .OrderByDescending(x => x.PorcentajePerdida)
            .First();

        var asignaturas = registros
            .Select(registro => new PerdidaAsignaturaDto
            {
                Id = registro.Id,
                ProgramaId = registro.ProgramaId,
                PeriodoId = registro.PeriodoId,
                Asignatura = registro.Asignatura,
                Matriculados = registro.Matriculados,
                Aprobados = registro.Aprobados,
                Reprobados = registro.Reprobados,
                PorcentajePerdida = registro.PorcentajePerdida
            })
            .ToList();

        return new AnalisisPerdidaAsignaturaDto
        {
            ProgramaId = programaId,
            PeriodoId = periodoId,
            TotalAsignaturas = registros.Count,
            TotalMatriculados = totalMatriculados,
            TotalAprobados = totalAprobados,
            TotalReprobados = totalReprobados,
            PorcentajePerdidaGeneral = porcentajePerdidaGeneral,
            AsignaturaMayorPerdida = mayorPerdida.Asignatura,
            MayorPorcentajePerdida = mayorPerdida.PorcentajePerdida,
            Asignaturas = asignaturas
        };
    }

    private static void ValidarParametros(
        int programaId,
        int periodoId)
    {
        if (programaId <= 0)
        {
            throw new ArgumentException(
                "El programa debe ser válido.",
                nameof(programaId));
        }

        if (periodoId <= 0)
        {
            throw new ArgumentException(
                "El periodo debe ser válido.",
                nameof(periodoId));
        }
    }
}