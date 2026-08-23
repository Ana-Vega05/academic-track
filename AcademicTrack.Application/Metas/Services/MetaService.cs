using AcademicTrack.Application.Metas.DTOs;
using AcademicTrack.Application.Metas.Interfaces;
using AcademicTrack.Domain.Entities;
using AcademicTrack.Domain.Enums;

namespace AcademicTrack.Application.Metas.Services;

public class MetaService
{
    private readonly IMetaRepository _metaRepository;
    private readonly IMetaEvidenciaRepository  _evidenciaRepository;
    private readonly IIndicadorRepository  _indicadorRepository;

    public MetaService(IMetaRepository metaRepository, IMetaEvidenciaRepository evidenciaRepository,
        IIndicadorRepository indicadorRepository)
    {
        _metaRepository = metaRepository;
        _evidenciaRepository = evidenciaRepository;
        _indicadorRepository = indicadorRepository;
    }

    public async Task<IReadOnlyList<MetaDto>> ObtenerPorProgramaAsync(
        int programaId, CancellationToken cancellationToken = default)
    {
        var metas = await _metaRepository.ObtenerPorProgramaAsync(programaId, cancellationToken);
        var resultado = new List<MetaDto>();
        foreach (var meta in metas)
        {
            resultado.Add(await MapearAsync(meta, cancellationToken));
        }
        return resultado;
    }
    
    public async Task<MetaDto> CrearAsync(CrearMetaDto dto, CancellationToken cancellationToken = default)
    {
        ValidarCrearMeta(dto);

        var meta = new Meta
        {
            ProgramaId = dto.ProgramaId,
            IndicadorId = dto.IndicadorId,
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Responsable = dto.Responsable,
            Periodicidad = Enum.Parse<PeriodicidadMeta>(dto.Periodicidad),
            FechaInicio = dto.FechaInicio,
            FechaLimite = dto.FechaLimite,
            ValorInicial = dto.ValorInicial,
            ValorEsperado = dto.ValorEsperado,
            AvanceActual = dto.ValorInicial,
            Estado = EstadoMeta.NoIniciada
        };

        var creada = await _metaRepository.CrearAsync(meta, cancellationToken);
        return await MapearAsync(creada, cancellationToken);
    }
    
    public async Task<MetaDto?> ActualizarAvanceAsync(
        int metaId, ActualizarAvanceMetaDto dto, CancellationToken cancellationToken = default)
    {
        var meta = await _metaRepository.ObtenerPorIdAsync(metaId, cancellationToken);
        if (meta is null) return null;

        meta.AvanceActual = dto.AvanceActual;

        if (!string.IsNullOrWhiteSpace(dto.Estado))
        {
            meta.Estado = Enum.Parse<EstadoMeta>(dto.Estado);
        }
        else if (meta.Estado is not (EstadoMeta.Cancelada or EstadoMeta.Cumplida))
        {
            var indicador = await _indicadorRepository.ObtenerPorIdAsync(meta.IndicadorId, cancellationToken)
                ?? throw new InvalidOperationException($"El indicador {meta.IndicadorId} no existe.");
            var cumplimiento = CalcularCumplimiento(meta, indicador);

            meta.Estado = cumplimiento >= 100
                ? EstadoMeta.Cumplida
                : DateOnly.FromDateTime(DateTime.UtcNow) > meta.FechaLimite
                    ? EstadoMeta.Retrasada
                    : EstadoMeta.EnProgreso;
        }

        await _metaRepository.ActualizarAsync(meta, cancellationToken);
        return await MapearAsync(meta, cancellationToken);
    }

    private async Task<MetaDto> MapearAsync(Meta meta, CancellationToken cancellationToken)
    {
        var indicador = await _indicadorRepository.ObtenerPorIdAsync(meta.IndicadorId, cancellationToken)
            ?? throw new InvalidOperationException($"El indicador {meta.IndicadorId} de la meta {meta.Id} no existe.");

        var evidencias = await _evidenciaRepository.ObtenerPorMetaAsync(meta.Id, cancellationToken);
        var cumplimiento = CalcularCumplimiento(meta, indicador);
        var semaforo = CalcularSemaforo(meta, cumplimiento);

        return new MetaDto
        {
            Id = meta.Id,
            ProgramaId = meta.ProgramaId,
            IndicadorNombre = indicador.Nombre,
            Nombre = meta.Nombre,
            Descripcion = meta.Descripcion,
            Responsable = meta.Responsable,
            Periodicidad = meta.Periodicidad.ToString(),
            FechaInicio = meta.FechaInicio,
            FechaLimite = meta.FechaLimite,
            ValorInicial = meta.ValorInicial,
            ValorEsperado = meta.ValorEsperado,
            AvanceActual = meta.AvanceActual,
            Estado = meta.Estado.ToString(),
            PorcentajeCumplimiento = cumplimiento,
            Semaforo = semaforo,
            Evidencias = evidencias
                .Select(e => new MetaEvidenciaDto { Descripcion = e.Descripcion, Url = e.Url, FechaCarga = e.FechaCarga })
                .ToList()
        };
    }
    
    /// <summary>
    /// Normaliza el avance sin importar si el indicador es Ascendente o Descendente:
    /// 0% = sin avance desde ValorInicial, 100% = ValorEsperado alcanzado, más de 100% = se superó la meta.
    /// </summary>
    private static decimal CalcularCumplimiento(Meta meta, Indicador indicador)
    {
        var rango = indicador.Direccion == DireccionIndicador.Ascendente
            ? meta.ValorEsperado - meta.ValorInicial
            : meta.ValorInicial - meta.ValorEsperado;

        if (rango == 0) return 100; // ya arrancaba en la meta

        var avance = indicador.Direccion == DireccionIndicador.Ascendente
            ? meta.AvanceActual - meta.ValorInicial
            : meta.ValorInicial - meta.AvanceActual;

        return Math.Round(avance / rango * 100, 2);
    }
    
    private static string CalcularSemaforo(Meta meta, decimal cumplimiento)
    {
        if (meta.Estado == EstadoMeta.Cancelada) return "Gris";
        if (meta.Estado == EstadoMeta.Cumplida || cumplimiento >= 100) return "Verde";

        var vencida = DateOnly.FromDateTime(DateTime.UtcNow) > meta.FechaLimite;
        if (vencida || meta.Estado == EstadoMeta.Retrasada) return "Rojo";

        return cumplimiento >= 40 ? "Amarillo" : "Rojo"; // umbral a validar con el equipo
    }

    private static void ValidarCrearMeta(CrearMetaDto dto)
    {
        if (dto.ProgramaId <= 0) throw new ArgumentException("El programa debe ser válido.", nameof(dto.ProgramaId));
        if (dto.IndicadorId <= 0) throw new ArgumentException("El indicador debe ser válido.", nameof(dto.IndicadorId));
        if (string.IsNullOrWhiteSpace(dto.Nombre)) throw new ArgumentException("El nombre es obligatorio.", nameof(dto.Nombre));
        if (string.IsNullOrWhiteSpace(dto.Responsable)) throw new ArgumentException("El responsable es obligatorio.", nameof(dto.Responsable));
        if (dto.FechaLimite < dto.FechaInicio) throw new ArgumentException("La fecha límite no puede ser anterior a la de inicio.", nameof(dto.FechaLimite));
    }
    
    public async Task<MetaDto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var meta = await _metaRepository.ObtenerPorIdAsync(id, cancellationToken);
        return meta is null ? null : await MapearAsync(meta, cancellationToken);
    }
    
    public async Task<IReadOnlyList<MetaDto>> ObtenerTodasAsync(CancellationToken cancellationToken = default)
    {
        var metas = await _metaRepository.ObtenerTodasAsync(cancellationToken);
        var resultado = new List<MetaDto>();
        foreach (var meta in metas)
            resultado.Add(await MapearAsync(meta, cancellationToken));
        return resultado;
    }
    
    public async Task<ResumenMetasDto> ObtenerResumenAsync(CancellationToken cancellationToken = default)
    {
        var metas = await ObtenerTodasAsync(cancellationToken);
        var porPrograma = metas
            .GroupBy(m => m.ProgramaId)
            .Select(g => new ResumenProgramaDto
            {
                ProgramaId = g.Key,
                Total = g.Count(),
                Verde = g.Count(m => m.Semaforo == "Verde"),
                Amarillo = g.Count(m => m.Semaforo == "Amarillo"),
                Rojo = g.Count(m => m.Semaforo == "Rojo"),
                Gris = g.Count(m => m.Semaforo == "Gris")
            })
            .OrderBy(r => r.ProgramaId)
            .ToList();

        return new ResumenMetasDto { PorPrograma = porPrograma };
    }
    
    public async Task<MetaDto?> ActualizarMetaAsync(int id, ActualizarMetaDto dto, CancellationToken cancellationToken = default)
    {
        var meta = await _metaRepository.ObtenerPorIdAsync(id, cancellationToken);
        if (meta is null) return null;

        ValidarActualizarMeta(dto);

        meta.Nombre = dto.Nombre;
        meta.Descripcion = dto.Descripcion;
        meta.Responsable = dto.Responsable;
        meta.Periodicidad = Enum.Parse<PeriodicidadMeta>(dto.Periodicidad);
        meta.FechaInicio = dto.FechaInicio;
        meta.FechaLimite = dto.FechaLimite;
        meta.ValorEsperado = dto.ValorEsperado;

        await _metaRepository.ActualizarAsync(meta, cancellationToken);
        return await MapearAsync(meta, cancellationToken);
    }
    
    public async Task<MetaDto?> CancelarAsync(int id, CancellationToken cancellationToken = default)
    {
        var meta = await _metaRepository.ObtenerPorIdAsync(id, cancellationToken);
        if (meta is null) return null;

        meta.Estado = EstadoMeta.Cancelada;
        await _metaRepository.ActualizarAsync(meta, cancellationToken);
        return await MapearAsync(meta, cancellationToken);
    }
    
    public async Task<MetaDto?> AgregarEvidenciaAsync(int metaId, CrearMetaEvidenciaDto dto, CancellationToken cancellationToken = default)
    {
        var meta = await _metaRepository.ObtenerPorIdAsync(metaId, cancellationToken);
        if (meta is null) return null;

        if (string.IsNullOrWhiteSpace(dto.Descripcion))
            throw new ArgumentException("La descripción de la evidencia es obligatoria.", nameof(dto.Descripcion));

        var evidencia = new MetaEvidencia
        {
            MetaId = metaId,
            Descripcion = dto.Descripcion,
            Url = dto.Url,
            FechaCarga = dto.FechaCarga ?? DateOnly.FromDateTime(DateTime.UtcNow)
        };

        await _evidenciaRepository.AgregarAsync(evidencia, cancellationToken);
        return await MapearAsync(meta, cancellationToken); // devuelve la meta con la evidencia ya incluida
    }
    
    private static void ValidarActualizarMeta(ActualizarMetaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre)) throw new ArgumentException("El nombre es obligatorio.", nameof(dto.Nombre));
        if (string.IsNullOrWhiteSpace(dto.Responsable)) throw new ArgumentException("El responsable es obligatorio.", nameof(dto.Responsable));
        if (dto.FechaLimite < dto.FechaInicio) throw new ArgumentException("La fecha límite no puede ser anterior a la de inicio.", nameof(dto.FechaLimite));
    }
    
}