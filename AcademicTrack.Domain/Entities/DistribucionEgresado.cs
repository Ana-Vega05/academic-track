namespace AcademicTrack.Domain.Entities;

public class DistribucionEgresado
{
    public int Id { get; set; }

    public int SeguimientoEgresadoId { get; set; }

    public string Tipo { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public int Cantidad { get; set; }
}