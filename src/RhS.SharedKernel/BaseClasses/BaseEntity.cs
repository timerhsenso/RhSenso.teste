using RhS.SharedKernel.Interfaces;

namespace RhS.SharedKernel.BaseClasses;

/// <summary>
/// Classe base para todas as entidades do sistema
/// Implementa multi-tenancy e auditoria
/// </summary>
public abstract class BaseEntity : ITenantEntity, IAuditableEntity
{
    /// <summary>
    /// Identificador único da entidade
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identificador único do tenant (cliente/empresa)
    /// </summary>
    public int IdSaas { get; set; }

    /// <summary>
    /// Data de criação do registro
    /// </summary>
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Usuário que criou o registro
    /// </summary>
    public string CriadoPor { get; set; } = string.Empty;

    /// <summary>
    /// Data da última atualização do registro
    /// </summary>
    public DateTime? AtualizadoEm { get; set; }

    /// <summary>
    /// Usuário que fez a última atualização do registro
    /// </summary>
    public string? AtualizadoPor { get; set; }

    /// <summary>
    /// Indica se o registro está ativo
    /// </summary>
    public bool Ativo { get; set; } = true;
}

