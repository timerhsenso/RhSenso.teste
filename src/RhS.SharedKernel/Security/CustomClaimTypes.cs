namespace RhS.SharedKernel.Security;

/// <summary>
/// Tipos de claims customizados do sistema
/// </summary>
public static class CustomClaimTypes
{
    /// <summary>
    /// Claim para o ID do tenant
    /// </summary>
    public const string TenantId = "tenant_id";

    /// <summary>
    /// Claim para permissões do usuário
    /// Formato: perm:{Modulo}.{Recurso}:{Acao}
    /// Exemplo: perm:RHU.Funcionario:Create
    /// </summary>
    public const string Permission = "permission";

    /// <summary>
    /// Claim para o nome da empresa/tenant
    /// </summary>
    public const string TenantName = "tenant_name";

    /// <summary>
    /// Claim para o tipo de usuário (Admin, User, etc.)
    /// </summary>
    public const string UserType = "user_type";

    /// <summary>
    /// Claim para o departamento do usuário
    /// </summary>
    public const string Department = "department";

    /// <summary>
    /// Claim para a filial do usuário
    /// </summary>
    public const string Branch = "branch";
}

