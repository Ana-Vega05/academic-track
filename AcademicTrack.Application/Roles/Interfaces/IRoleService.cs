using AcademicTrack.Application.Roles.DTOs;

namespace AcademicTrack.Application.Roles.Interfaces;

public interface IRoleService
{
    Task<IReadOnlyList<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken = default);
    Task<RoleDto?> GetRoleByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<RoleDto> CreateRoleAsync(CreateRoleDto dto, CancellationToken cancellationToken = default);
    Task<RoleDto> UpdateRoleAsync(int id, UpdateRoleDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteRoleAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PermissionDto>> GetAllPermissionsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<UserRoleDto>> GetAllUsersWithRolesAsync(CancellationToken cancellationToken = default);
    Task<UserRoleDto> AssignRoleToUserAsync(int userId, int roleId, CancellationToken cancellationToken = default);
    Task<UserRoleDto> CreateUserAsync(CreateUserDto dto, CancellationToken cancellationToken = default);
}
