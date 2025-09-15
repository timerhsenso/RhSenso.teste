namespace RhS.SharedKernel.Interfaces;

/// <summary>
/// Interface para entidades que suportam multi-tenancy
/// </summary>
public interface ITenantEntity
{
    /// <summary>
    /// Identificador único do tenant (cliente/empresa)
    /// </summary>
    int IdSaas { get; set; }
}

