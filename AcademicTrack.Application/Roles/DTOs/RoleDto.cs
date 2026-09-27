namespace AcademicTrack.Application.Roles.DTOs;

public class RoleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsSystemRole { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UsersCount { get; set; }
    public List<PermissionDto> Permissions { get; set; } = new();
}
