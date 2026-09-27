using AcademicTrack.Application.Roles.DTOs;
using AcademicTrack.Application.Roles.Interfaces;
using AcademicTrack.Domain.Entities;
using AcademicTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AcademicTrack.Infrastructure.Services;

public class RoleService : IRoleService
{
    private readonly AcademicTrackDbContext _context;

    public RoleService(AcademicTrackDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken = default)
    {
        var roles = await _context.Roles
            .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
            .Include(r => r.Users)
            .AsNoTracking()
            .OrderBy(r => r.Id)
            .ToListAsync(cancellationToken);

        return roles.Select(MapToRoleDto).ToList();
    }

    public async Task<RoleDto?> GetRoleByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var role = await _context.Roles
            .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
            .Include(r => r.Users)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        return role == null ? null : MapToRoleDto(role);
    }

    public async Task<RoleDto> CreateRoleAsync(CreateRoleDto dto, CancellationToken cancellationToken = default)
    {
        var trimmedName = dto.Name?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(trimmedName))
        {
            throw new ArgumentException("El nombre del rol es requerido.");
        }

        var exists = await _context.Roles.AnyAsync(r => r.Name.ToLower() == trimmedName.ToLower(), cancellationToken);
        if (exists)
        {
            throw new InvalidOperationException($"Ya existe un rol con el nombre '{trimmedName}'.");
        }

        var role = new Role
        {
            Name = trimmedName,
            Description = dto.Description?.Trim() ?? string.Empty,
            IsSystemRole = false,
            CreatedAt = DateTime.UtcNow
        };

        if (dto.PermissionIds != null && dto.PermissionIds.Count > 0)
        {
            var validPermissions = await _context.Permissions
                .Where(p => dto.PermissionIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync(cancellationToken);

            foreach (var permId in validPermissions)
            {
                role.RolePermissions.Add(new RolePermission
                {
                    PermissionId = permId
                });
            }
        }

        await _context.Roles.AddAsync(role, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return (await GetRoleByIdAsync(role.Id, cancellationToken))!;
    }

    public async Task<RoleDto> UpdateRoleAsync(int id, UpdateRoleDto dto, CancellationToken cancellationToken = default)
    {
        var role = await _context.Roles
            .Include(r => r.RolePermissions)
            .Include(r => r.Users)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (role == null)
        {
            throw new KeyNotFoundException($"El rol con ID {id} no fue encontrado.");
        }

        var trimmedName = dto.Name?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(trimmedName))
        {
            throw new ArgumentException("El nombre del rol es requerido.");
        }

        // Si cambia el nombre, validar que no esté duplicado
        if (!role.Name.Equals(trimmedName, StringComparison.OrdinalIgnoreCase))
        {
            var exists = await _context.Roles.AnyAsync(r => r.Id != id && r.Name.ToLower() == trimmedName.ToLower(), cancellationToken);
            if (exists)
            {
                throw new InvalidOperationException($"Ya existe otro rol con el nombre '{trimmedName}'.");
            }

            role.Name = trimmedName;

            // Actualizar nombre denormalizado en usuarios
            foreach (var user in role.Users)
            {
                user.Role = trimmedName;
            }
        }

        role.Description = dto.Description?.Trim() ?? string.Empty;

        // Actualizar permisos
        _context.RolePermissions.RemoveRange(role.RolePermissions);

        if (dto.PermissionIds != null && dto.PermissionIds.Count > 0)
        {
            var validPermissions = await _context.Permissions
                .Where(p => dto.PermissionIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync(cancellationToken);

            foreach (var permId in validPermissions)
            {
                role.RolePermissions.Add(new RolePermission
                {
                    RoleId = role.Id,
                    PermissionId = permId
                });
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return (await GetRoleByIdAsync(role.Id, cancellationToken))!;
    }

    public async Task<bool> DeleteRoleAsync(int id, CancellationToken cancellationToken = default)
    {
        var role = await _context.Roles
            .Include(r => r.Users)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (role == null)
        {
            return false;
        }

        if (role.IsSystemRole)
        {
            throw new InvalidOperationException("No se pueden eliminar los roles predeterminados del sistema.");
        }

        if (role.Users.Count > 0)
        {
            throw new InvalidOperationException($"No se puede eliminar el rol '{role.Name}' porque tiene {role.Users.Count} usuario(s) asignado(s). Reasigna los usuarios a otro rol primero.");
        }

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IReadOnlyList<PermissionDto>> GetAllPermissionsAsync(CancellationToken cancellationToken = default)
    {
        var permissions = await _context.Permissions
            .AsNoTracking()
            .OrderBy(p => p.Module)
            .ThenBy(p => p.Name)
            .ToListAsync(cancellationToken);

        return permissions.Select(p => new PermissionDto
        {
            Id = p.Id,
            Code = p.Code,
            Name = p.Name,
            Module = p.Module,
            Description = p.Description
        }).ToList();
    }

    public async Task<IReadOnlyList<UserRoleDto>> GetAllUsersWithRolesAsync(CancellationToken cancellationToken = default)
    {
        var users = await _context.Users
            .Include(u => u.UserRole)
            .AsNoTracking()
            .OrderBy(u => u.FullName)
            .ToListAsync(cancellationToken);

        return users.Select(u => new UserRoleDto
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            FullName = u.FullName,
            RoleId = u.RoleId,
            RoleName = u.UserRole?.Name ?? u.Role,
            IsActive = u.IsActive
        }).ToList();
    }

    public async Task<UserRoleDto> AssignRoleToUserAsync(int userId, int roleId, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user == null)
        {
            throw new KeyNotFoundException($"Usuario con ID {userId} no encontrado.");
        }

        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);
        if (role == null)
        {
            throw new KeyNotFoundException($"Rol con ID {roleId} no encontrado.");
        }

        user.RoleId = role.Id;
        user.Role = role.Name;

        await _context.SaveChangesAsync(cancellationToken);

        return new UserRoleDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            RoleId = user.RoleId,
            RoleName = role.Name,
            IsActive = user.IsActive
        };
    }

    private static RoleDto MapToRoleDto(Role role)
    {
        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            IsSystemRole = role.IsSystemRole,
            CreatedAt = role.CreatedAt,
            UsersCount = role.Users?.Count ?? 0,
            Permissions = role.RolePermissions
                .Where(rp => rp.Permission != null)
                .Select(rp => new PermissionDto
                {
                    Id = rp.Permission.Id,
                    Code = rp.Permission.Code,
                    Name = rp.Permission.Name,
                    Module = rp.Permission.Module,
                    Description = rp.Permission.Description
                })
                .OrderBy(p => p.Module)
                .ThenBy(p => p.Name)
                .ToList()
        };
    }
}
