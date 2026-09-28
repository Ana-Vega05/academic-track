using AcademicTrack.API.Infrastructure;
using AcademicTrack.Application.Roles.DTOs;
using AcademicTrack.Application.Roles.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicTrack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Route("roles")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    [RequirePermission("ROLES_MANAGE", "USERS_ASSIGN")]
    public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
    {
        var roles = await _roleService.GetAllRolesAsync(cancellationToken);
        return Ok(roles);
    }

    [HttpGet("{id:int}")]
    [RequirePermission("ROLES_MANAGE")]
    public async Task<IActionResult> GetRoleById(int id, CancellationToken cancellationToken)
    {
        var role = await _roleService.GetRoleByIdAsync(id, cancellationToken);
        return role == null ? NotFound(new { message = $"El rol con ID {id} no existe." }) : Ok(role);
    }

    [HttpPost]
    [RequirePermission("ROLES_MANAGE")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _roleService.CreateRoleAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetRoleById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [RequirePermission("ROLES_MANAGE")]
    public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _roleService.UpdateRoleAsync(id, dto, cancellationToken);
            return Ok(updated);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [RequirePermission("ROLES_MANAGE")]
    public async Task<IActionResult> DeleteRole(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _roleService.DeleteRoleAsync(id, cancellationToken);
            return deleted ? NoContent() : NotFound(new { message = $"El rol con ID {id} no fue encontrado." });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet("permissions")]
    [RequirePermission("ROLES_MANAGE")]
    public async Task<IActionResult> GetAllPermissions(CancellationToken cancellationToken)
    {
        var permissions = await _roleService.GetAllPermissionsAsync(cancellationToken);
        return Ok(permissions);
    }

    [HttpGet("users")]
    [RequirePermission("USERS_ASSIGN", "ROLES_MANAGE")]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var users = await _roleService.GetAllUsersWithRolesAsync(cancellationToken);
        return Ok(users);
    }

    [HttpPut("users/{userId:int}")]
    [RequirePermission("USERS_ASSIGN", "ROLES_MANAGE")]
    public async Task<IActionResult> AssignRoleToUser(int userId, [FromBody] AssignUserRoleDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _roleService.AssignRoleToUserAsync(userId, dto.RoleId, cancellationToken);
            return Ok(user);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("users")]
    [RequirePermission("USERS_ASSIGN", "ROLES_MANAGE")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _roleService.CreateUserAsync(dto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, user);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
