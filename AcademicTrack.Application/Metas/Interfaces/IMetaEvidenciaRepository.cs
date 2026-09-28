using AcademicTrack.Domain.Entities;

namespace AcademicTrack.Application.Metas.Interfaces;

public interface IMetaEvidenciaRepository
{
    Task<IReadOnlyList<MetaEvidencia>> ObtenerPorMetaAsync(int metaId, CancellationToken cancellationToken = default);
    Task AgregarAsync(MetaEvidencia evidencia, CancellationToken cancellationToken = default);
}