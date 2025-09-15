using RhS.SharedKernel.Interfaces;

namespace RhS.SharedKernel.BaseClasses;

/// <summary>
/// Classe base para handlers do MediatR
/// Fornece acesso ao contexto do tenant e funcionalidades comuns
/// </summary>
public abstract class BaseHandler
{
    protected readonly ITenantAccessor _tenantAccessor;

    protected BaseHandler(ITenantAccessor tenantAccessor)
    {
        _tenantAccessor = tenantAccessor;
    }

    /// <summary>
    /// Obtém o ID do tenant atual
    /// </summary>
    protected int GetCurrentTenantId()
    {
        return _tenantAccessor.GetTenantId();
    }

    /// <summary>
    /// Obtém o nome do usuário atual
    /// </summary>
    protected string GetCurrentUser()
    {
        return _tenantAccessor.GetCurrentUser();
    }

    /// <summary>
    /// Verifica se há um tenant válido no contexto
    /// </summary>
    protected bool HasValidTenant()
    {
        return _tenantAccessor.HasValidTenant();
    }
}

