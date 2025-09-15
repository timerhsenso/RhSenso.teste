using RhS.SharedKernel.BaseClasses;

namespace RhS.RHU.Core.Entities;

/// <summary>
/// Entidade que representa um funcionário no sistema
/// </summary>
public class Funcionario : BaseEntity
{
    /// <summary>
    /// Nome completo do funcionário
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Email do funcionário
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// CPF do funcionário
    /// </summary>
    public string CPF { get; set; } = string.Empty;

    /// <summary>
    /// Salário base do funcionário
    /// </summary>
    public decimal Salario { get; set; }

    /// <summary>
    /// Data de admissão do funcionário
    /// </summary>
    public DateTime DataAdmissao { get; set; }

    /// <summary>
    /// Cargo do funcionário
    /// </summary>
    public string Cargo { get; set; } = string.Empty;

    /// <summary>
    /// Departamento do funcionário
    /// </summary>
    public string Departamento { get; set; } = string.Empty;

    /// <summary>
    /// Status do funcionário (Ativo, Inativo, Demitido, etc.)
    /// </summary>
    public string Status { get; set; } = "Ativo";
}

