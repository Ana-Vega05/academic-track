using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace AcademicTrack.API.Infrastructure;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class RequirePermissionAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string[] _permissions;

    public RequirePermissionAttribute(params string[] permissions)
    {
        _permissions = permissions;
    }

    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        // 1. Verificar si está autenticado
        if (!user.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                message = "Debe iniciar sesión para acceder a este recurso."
            });
            return Task.CompletedTask;
        }

        // 2. Administrador tiene acceso total a cualquier recurso
        var role = user.FindFirstValue(ClaimTypes.Role) ?? user.FindFirstValue("role");
        if (string.Equals(role, "Administrador", StringComparison.OrdinalIgnoreCase) || user.IsInRole("Administrador"))
        {
            return Task.CompletedTask;
        }

        // 3. Si no se especificaron permisos requeridos, con estar autenticado es suficiente
        if (_permissions == null || _permissions.Length == 0)
        {
            return Task.CompletedTask;
        }

        // 4. Validar si el usuario posee al menos uno de los permisos requeridos
        var userPermissions = user.FindAll("permission").Select(c => c.Value).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var hasAccess = _permissions.Any(p => userPermissions.Contains(p));

        if (!hasAccess)
        {
            context.Result = new ObjectResult(new
            {
                message = "Acceso denegado. No tiene los permisos necesarios para realizar esta acción.",
                requiredPermissions = _permissions
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }

        return Task.CompletedTask;
    }
}
