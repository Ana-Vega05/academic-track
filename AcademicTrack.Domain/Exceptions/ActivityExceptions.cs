namespace AcademicTrack.Domain.Exceptions;

public static class ActivityExceptions
{
    public static void ValidateActivity(int programId, string name, string responsible, string? location)
    {
        if (programId <= 0) throw new ArgumentException("El programa debe ser válido.", nameof(programId));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("El nombre es obligatorio.", nameof(name));
        if (string.IsNullOrWhiteSpace(responsible)) throw new ArgumentException("El responsable es obligatorio.", nameof(responsible));
        ValidateLength(name, 300, nameof(name));
        ValidateLength(responsible, 150, nameof(responsible));
        if (location is not null) ValidateLength(location, 200, nameof(location));
    }

    public static void ValidateLength(string value, int max, string field)
    {
        if (value.Length > max)
            throw new ArgumentException($"{field} no puede superar {max} caracteres.", field);
    }
}
