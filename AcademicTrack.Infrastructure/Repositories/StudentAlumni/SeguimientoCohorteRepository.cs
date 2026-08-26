using AcademicTrack.Application.StudentAlumni.Cohortes.DTOs;
using AcademicTrack.Application.StudentAlumni.Cohortes.Interfaces;
using AcademicTrack.Domain.Entities;
using AcademicTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AcademicTrack.Infrastructure.Repositories.StudentAlumni;

public class SeguimientoCohorteRepository : ISeguimientoCohorteRepository
{
    private readonly AcademicTrackDbContext _context;

    public SeguimientoCohorteRepository(
        AcademicTrackDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<SeguimientoCohorte>> ObtenerPorCohorteAsync(
        int programaId,
        int periodoCohorteId,
        CancellationToken cancellationToken = default)
    {
        return await _context.SeguimientosCohorte
            .AsNoTracking()
            .Where(x =>
                x.ProgramaId == programaId &&
                x.PeriodoCohorteId == periodoCohorteId)
            .OrderBy(x => x.SemestreSeguimiento)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SeguimientoCohorte>> ObtenerPorProgramaAsync(
        int programaId,
        CancellationToken cancellationToken = default)
    {
        return await _context.SeguimientosCohorte
            .AsNoTracking()
            .Where(x => x.ProgramaId == programaId)
            .OrderBy(x => x.PeriodoCohorteId)
            .ThenBy(x => x.SemestreSeguimiento)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SeguimientoCohorteConPeriodoDto>> ObtenerComparacionAsync(
        int programaId,
        CancellationToken cancellationToken = default)
    {
        return await (
            from seguimiento in _context.SeguimientosCohorte
            join periodo in _context.PeriodoAcademicos
                on seguimiento.PeriodoCohorteId equals periodo.Id
            where seguimiento.ProgramaId == programaId
            orderby seguimiento.PeriodoCohorteId,
                     seguimiento.SemestreSeguimiento
            select new SeguimientoCohorteConPeriodoDto
            {
                PeriodoCohorteId = seguimiento.PeriodoCohorteId,
                Anio = periodo.Anio,
                Semestre = periodo.Semestre.ToString(),
                SemestreSeguimiento = seguimiento.SemestreSeguimiento,
                Ingresaron = seguimiento.Ingresaron,
                Continuaron = seguimiento.Continuaron,
                Desertores = seguimiento.Desertores,
                Graduados = seguimiento.Graduados
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SeguimientoCohorteConPeriodoDto>> ObtenerConPeriodoAsync(
        int programaId,
        CancellationToken cancellationToken = default)
    {
        return await (
            from seguimiento in _context.SeguimientosCohorte
            join periodo in _context.PeriodoAcademicos
                on seguimiento.PeriodoCohorteId equals periodo.Id
            where seguimiento.ProgramaId == programaId
            orderby periodo.Anio,
                     periodo.Semestre,
                     seguimiento.SemestreSeguimiento
            select new SeguimientoCohorteConPeriodoDto
            {
                PeriodoCohorteId = seguimiento.PeriodoCohorteId,
                Anio = periodo.Anio,
                Semestre = periodo.Semestre.ToString(),
                SemestreSeguimiento = seguimiento.SemestreSeguimiento,
                Ingresaron = seguimiento.Ingresaron,
                Continuaron = seguimiento.Continuaron,
                Desertores = seguimiento.Desertores,
                Graduados = seguimiento.Graduados
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}