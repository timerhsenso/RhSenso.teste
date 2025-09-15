namespace RhS.SharedKernel.Interfaces;

/// <summary>
/// Interface para acesso ao contexto do tenant atual
/// </summary>
public interface ITenantAccessor
{
    /// <summary>
    /// Obtém o ID do tenant atual
    /// </summary>
    int GetTenantId();

    /// <summary>
    /// Obtém o nome do usuário atual
    /// </summary>
    string GetCurrentUser();

    /// <summary>
    /// Verifica se há um tenant válido no contexto
    /// </summary>
    bool HasValidTenant();
}

