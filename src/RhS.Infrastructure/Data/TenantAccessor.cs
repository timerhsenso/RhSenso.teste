using Microsoft.AspNetCore.Http;
using RhS.SharedKernel.Extensions;
using RhS.SharedKernel.Interfaces;

namespace RhS.Infrastructure.Data;

/// <summary>
/// Implementação do ITenantAccessor para acesso ao contexto do tenant atual
/// </summary>
public class TenantAccessor : ITenantAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Obtém o ID do tenant atual do contexto HTTP
    /// </summary>
    public int GetTenantId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.GetTenantId() ?? 0;
    }

    /// <summary>
    /// Obtém o nome do usuário atual do contexto HTTP
    /// </summary>
    public string GetCurrentUser()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.GetUserName() ?? "System";
    }

    /// <summary>
    /// Verifica se há um tenant válido no contexto
    /// </summary>
    public bool HasValidTenant()
    {
        return GetTenantId() > 0;
    }
}

