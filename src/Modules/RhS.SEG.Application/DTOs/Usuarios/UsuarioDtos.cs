using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RhS.SEG.Application.DTOs.Usuarios;

/// <summary>
/// DTO para listagem de usuários - usado em grids e consultas
/// </summary>
public sealed record UsuarioListDto(
    [property: JsonPropertyName("codigo")] string Codigo,
    [property: JsonPropertyName("cdUsuario")] string CdUsuario,
    [property: JsonPropertyName("descricao")] string Descricao,
    [property: JsonPropertyName("tipoStr")] string? TipoStr,
    [property: JsonPropertyName("email")] string? Email,
    [property: JsonPropertyName("situacao")] string Situacao,
    [property: JsonPropertyName("noUser")] int NoUser,
    [property: JsonPropertyName("cdEmpresa")] int? CdEmpresa,
    [property: JsonPropertyName("cdFilial")] int? CdFilial
);

/// <summary>
/// DTO para criação de novos usuários
/// </summary>
public sealed class UsuarioCreateDto
{
    /// <summary>
    /// Código/Login do usuário (até 30 caracteres)
    /// </summary>
    [JsonPropertyName("codigo")]
    [Required(ErrorMessage = "Código do usuário é obrigatório")]
    [StringLength(30, ErrorMessage = "Código deve ter no máximo 30 caracteres")]
    public string Codigo { get; set; } = string.Empty;

    /// <summary>
    /// Nome/Descrição do usuário (até 50 caracteres)
    /// </summary>
    [JsonPropertyName("descricao")]
    [Required(ErrorMessage = "Descrição do usuário é obrigatória")]
    [StringLength(50, ErrorMessage = "Descrição deve ter no máximo 50 caracteres")]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Tipo do usuário (0=Prestador de Serviço, 1=Empregado)
    /// </summary>
    [JsonPropertyName("tipo")]
    [Range(0, 1, ErrorMessage = "Tipo deve ser 0 (Prestador) ou 1 (Empregado)")]
    public int Tipo { get; set; }

    /// <summary>
    /// Senha do usuário (até 20 caracteres)
    /// </summary>
    [JsonPropertyName("senhaUser")]
    [StringLength(20, ErrorMessage = "Senha deve ter no máximo 20 caracteres")]
    public string? SenhaUser { get; set; }

    /// <summary>
    /// Nome para impressão em cheques (até 50 caracteres)
    /// </summary>
    [JsonPropertyName("nomeImpCheque")]
    [StringLength(50, ErrorMessage = "Nome para cheque deve ter no máximo 50 caracteres")]
    public string? NomeImpCheque { get; set; }

    /// <summary>
    /// Número da matrícula (até 8 caracteres)
    /// </summary>
    [JsonPropertyName("noMatric")]
    [StringLength(8, ErrorMessage = "Matrícula deve ter no máximo 8 caracteres")]
    public string? NoMatric { get; set; }

    /// <summary>
    /// Código da empresa
    /// </summary>
    [JsonPropertyName("cdEmpresa")]
    public int? CdEmpresa { get; set; }

    /// <summary>
    /// Código da filial
    /// </summary>
    [JsonPropertyName("cdFilial")]
    public int? CdFilial { get; set; }

    /// <summary>
    /// Número do usuário (identificador único)
    /// </summary>
    [JsonPropertyName("noUser")]
    [Range(1, int.MaxValue, ErrorMessage = "NoUser deve ser maior que 0")]
    public int NoUser { get; set; }

    /// <summary>
    /// Email do usuário (até 100 caracteres)
    /// </summary>
    [JsonPropertyName("email")]
    [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
    [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
    public string? Email { get; set; }

    /// <summary>
    /// Status ativo/inativo (S=Ativo, N=Inativo)
    /// </summary>
    [JsonPropertyName("ativo")]
    [Required(ErrorMessage = "Status ativo é obrigatório")]
    [RegularExpression("^[SN]$", ErrorMessage = "Ativo deve ser 'S' ou 'N'")]
    public string Ativo { get; set; } = "S";

    /// <summary>
    /// Nome de usuário normalizado (até 30 caracteres)
    /// </summary>
    [JsonPropertyName("normalizedUserName")]
    [StringLength(30, ErrorMessage = "NormalizedUserName deve ter no máximo 30 caracteres")]
    public string? NormalizedUserName { get; set; }

    /// <summary>
    /// ID do funcionário associado
    /// </summary>
    [JsonPropertyName("idFuncionario")]
    public Guid? IdFuncionario { get; set; }

    /// <summary>
    /// Flag para não receber emails (S=Não recebe, N=Recebe, null=Padrão)
    /// </summary>
    [JsonPropertyName("flNaoRecebeEmail")]
    [RegularExpression("^[SN]$", ErrorMessage = "FlNaoRecebeEmail deve ser 'S' ou 'N'")]
    public string? FlNaoRecebeEmail { get; set; }
}

/// <summary>
/// DTO para atualização de usuários existentes
/// </summary>
public sealed class UsuarioUpdateDto
{
    /// <summary>
    /// Nome/Descrição do usuário (até 50 caracteres)
    /// </summary>
    [JsonPropertyName("descricao")]
    [Required(ErrorMessage = "Descrição do usuário é obrigatória")]
    [StringLength(50, ErrorMessage = "Descrição deve ter no máximo 50 caracteres")]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Tipo do usuário (0=Prestador de Serviço, 1=Empregado)
    /// </summary>
    [JsonPropertyName("tipo")]
    [Range(0, 1, ErrorMessage = "Tipo deve ser 0 (Prestador) ou 1 (Empregado)")]
    public int Tipo { get; set; }

    /// <summary>
    /// Senha do usuário (até 20 caracteres)
    /// </summary>
    [JsonPropertyName("senhaUser")]
    [StringLength(20, ErrorMessage = "Senha deve ter no máximo 20 caracteres")]
    public string? SenhaUser { get; set; }

    /// <summary>
    /// Nome para impressão em cheques (até 50 caracteres)
    /// </summary>
    [JsonPropertyName("nomeImpCheque")]
    [StringLength(50, ErrorMessage = "Nome para cheque deve ter no máximo 50 caracteres")]
    public string? NomeImpCheque { get; set; }

    /// <summary>
    /// Número da matrícula (até 8 caracteres)
    /// </summary>
    [JsonPropertyName("noMatric")]
    [StringLength(8, ErrorMessage = "Matrícula deve ter no máximo 8 caracteres")]
    public string? NoMatric { get; set; }

    /// <summary>
    /// Código da empresa
    /// </summary>
    [JsonPropertyName("cdEmpresa")]
    public int? CdEmpresa { get; set; }

    /// <summary>
    /// Código da filial
    /// </summary>
    [JsonPropertyName("cdFilial")]
    public int? CdFilial { get; set; }

    /// <summary>
    /// Número do usuário (identificador único)
    /// </summary>
    [JsonPropertyName("noUser")]
    [Range(1, int.MaxValue, ErrorMessage = "NoUser deve ser maior que 0")]
    public int NoUser { get; set; }

    /// <summary>
    /// Email do usuário (até 100 caracteres)
    /// </summary>
    [JsonPropertyName("email")]
    [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
    [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
    public string? Email { get; set; }

    /// <summary>
    /// Status ativo/inativo (S=Ativo, N=Inativo)
    /// </summary>
    [JsonPropertyName("ativo")]
    [Required(ErrorMessage = "Status ativo é obrigatório")]
    [RegularExpression("^[SN]$", ErrorMessage = "Ativo deve ser 'S' ou 'N'")]
    public string Ativo { get; set; } = "S";

    /// <summary>
    /// Nome de usuário normalizado (até 30 caracteres)
    /// </summary>
    [JsonPropertyName("normalizedUserName")]
    [StringLength(30, ErrorMessage = "NormalizedUserName deve ter no máximo 30 caracteres")]
    public string? NormalizedUserName { get; set; }

    /// <summary>
    /// ID do funcionário associado
    /// </summary>
    [JsonPropertyName("idFuncionario")]
    public Guid? IdFuncionario { get; set; }

    /// <summary>
    /// Flag para não receber emails (S=Não recebe, N=Recebe, null=Padrão)
    /// </summary>
    [JsonPropertyName("flNaoRecebeEmail")]
    [RegularExpression("^[SN]$", ErrorMessage = "FlNaoRecebeEmail deve ser 'S' ou 'N'")]
    public string? FlNaoRecebeEmail { get; set; }
}

/// <summary>
/// DTO para formulários de usuário - usado em telas de cadastro
/// </summary>
public sealed class UsuarioFormDto
{
    /// <summary>
    /// Código/Login do usuário
    /// </summary>
    public string CdUsuario { get; set; } = string.Empty;

    /// <summary>
    /// Nome/Descrição do usuário
    /// </summary>
    public string DcUsuario { get; set; } = string.Empty;

    /// <summary>
    /// Senha do usuário
    /// </summary>
    public string? SenhaUser { get; set; }

    /// <summary>
    /// Nome para impressão em cheques
    /// </summary>
    public string? NmImpCche { get; set; }

    /// <summary>
    /// Tipo do usuário
    /// </summary>
    public string? TpUsuario { get; set; }

    /// <summary>
    /// Número da matrícula
    /// </summary>
    public string? NoMatric { get; set; }

    /// <summary>
    /// Código da empresa
    /// </summary>
    public int? CdEmpresa { get; set; }

    /// <summary>
    /// Código da filial
    /// </summary>
    public int? CdFilial { get; set; }

    /// <summary>
    /// Número do usuário
    /// </summary>
    public int NoUser { get; set; }

    /// <summary>
    /// Email do usuário
    /// </summary>
    public string? EmailUsuario { get; set; }

    /// <summary>
    /// Status ativo/inativo
    /// </summary>
    public string FlAtivo { get; set; } = "S";

    /// <summary>
    /// Nome de usuário normalizado
    /// </summary>
    public string? NormalizedUserName { get; set; }

    /// <summary>
    /// ID do funcionário associado
    /// </summary>
    public Guid? IdFuncionario { get; set; }

    /// <summary>
    /// Flag para não receber emails
    /// </summary>
    public string? FlNaoRecebeEmail { get; set; }
}

