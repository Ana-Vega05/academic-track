namespace AcademicTrack.Domain.Entities;

public class PublicacionAutor
{
    public int PublicacionId { get; set; }
    public int AutorId { get; set; }
    public short? Orden { get; set; }
}