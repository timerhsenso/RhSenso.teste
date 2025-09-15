using Microsoft.AspNetCore.Authorization;

namespace RhS.SharedKernel.Security;

/// <summary>
/// Atributo para controle de permissões granulares
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class PermissionAttribute : AuthorizeAttribute
{
    /// <summary>
    /// Inicializa uma nova instância do atributo de permissão
    /// </summary>
    /// <param name="permission">Permissão necessária no formato {Modulo}.{Recurso}.{Acao}</param>
    public PermissionAttribute(string permission) : base(policy: permission)
    {
        Permission = permission;
    }

    /// <summary>
    /// Permissão necessária para acessar o recurso
    /// </summary>
    public string Permission { get; }
}

