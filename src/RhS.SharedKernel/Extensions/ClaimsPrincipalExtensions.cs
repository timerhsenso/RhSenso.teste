using System.Security.Claims;
using RhS.SharedKernel.Security;

namespace RhS.SharedKernel.Extensions;

/// <summary>
/// Extensões para ClaimsPrincipal para facilitar o acesso a informações do usuário
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Obtém o ID do tenant do usuário atual
    /// </summary>
    public static int GetTenantId(this ClaimsPrincipal user)
    {
        var tenantIdClaim = user.FindFirst(CustomClaimTypes.TenantId)?.Value;
        return int.TryParse(tenantIdClaim, out var tenantId) ? tenantId : 0;
    }

    /// <summary>
    /// Obtém o nome do usuário atual
    /// </summary>
    public static string GetUserName(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
    }

    /// <summary>
    /// Obtém o ID do usuário atual
    /// </summary>
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    /// <summary>
    /// Obtém o email do usuário atual
    /// </summary>
    public static string GetEmail(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
    }

    /// <summary>
    /// Verifica se o usuário tem uma permissão específica
    /// </summary>
    public static bool HasPermission(this ClaimsPrincipal user, string permission)
    {
        return user.HasClaim(CustomClaimTypes.Permission, permission);
    }

    /// <summary>
    /// Obtém todas as permissões do usuário
    /// </summary>
    public static IEnumerable<string> GetPermissions(this ClaimsPrincipal user)
    {
        return user.FindAll(CustomClaimTypes.Permission).Select(c => c.Value);
    }

    /// <summary>
    /// Verifica se o usuário está em um papel específico
    /// </summary>
    public static bool IsInRole(this ClaimsPrincipal user, string role)
    {
        return user.HasClaim(ClaimTypes.Role, role);
    }

    /// <summary>
    /// Obtém todos os papéis do usuário
    /// </summary>
    public static IEnumerable<string> GetRoles(this ClaimsPrincipal user)
    {
        return user.FindAll(ClaimTypes.Role).Select(c => c.Value);
    }
}

