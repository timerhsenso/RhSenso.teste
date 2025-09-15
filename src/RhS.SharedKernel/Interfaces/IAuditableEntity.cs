namespace RhS.SharedKernel.Interfaces;

/// <summary>
/// Interface para entidades que suportam auditoria
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    /// Data de criação do registro
    /// </summary>
    DateTime CriadoEm { get; set; }

    /// <summary>
    /// Usuário que criou o registro
    /// </summary>
    string CriadoPor { get; set; }

    /// <summary>
    /// Data da última atualização do registro
    /// </summary>
    DateTime? AtualizadoEm { get; set; }

    /// <summary>
    /// Usuário que fez a última atualização do registro
    /// </summary>
    string? AtualizadoPor { get; set; }

    /// <summary>
    /// Indica se o registro está ativo
    /// </summary>
    bool Ativo { get; set; }
}

