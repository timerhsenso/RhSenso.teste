using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using RhS.SharedKernel.Security;

namespace RhS.SEG.Application.DTOs.Botoes;

/// <summary>
/// DTO para chave composta de botão
/// </summary>
public sealed record BotaoKeyDto(
    [property: JsonPropertyName("codigoSistema")] string CodigoSistema,
    [property: JsonPropertyName("codigoFuncao")] string CodigoFuncao,
    [property: JsonPropertyName("nome")] string Nome
);

/// <summary>
/// DTO para listagem de botões - usado em grids e consultas
/// </summary>
public sealed record BotaoListDto(
    [property: JsonPropertyName("codigoSistema")] string CodigoSistema,
    [property: JsonPropertyName("codigoFuncao")] string CodigoFuncao,
    [property: JsonPropertyName("nome")] string Nome,
    [property: JsonPropertyName("descricao")] string Descricao,
    [property: JsonPropertyName("acao")] string Acao
)
{
    /// <summary>
    /// Chave composta raw (formato: Sistema|Funcao|Nome)
    /// </summary>
    [JsonPropertyName("keyRaw")]
    public string KeyRaw => KeyCodec.CreateCompositeKey(CodigoSistema, CodigoFuncao, Nome);

    /// <summary>
    /// ID seguro codificado em Base64Url
    /// </summary>
    [JsonPropertyName("id")]
    public string Id => KeyCodec.ToBase64Url(KeyRaw);
}

/// <summary>
/// DTO para criação de novos botões
/// </summary>
public sealed class BotaoCreateDto
{
    /// <summary>
    /// Código do sistema (obrigatório)
    /// </summary>
    [JsonPropertyName("codigoSistema")]
    [Required(ErrorMessage = "Código do sistema é obrigatório")]
    [StringLength(10, ErrorMessage = "Código do sistema deve ter no máximo 10 caracteres")]
    public string CodigoSistema { get; set; } = string.Empty;

    /// <summary>
    /// Código da função (obrigatório)
    /// </summary>
    [JsonPropertyName("codigoFuncao")]
    [Required(ErrorMessage = "Código da função é obrigatório")]
    [StringLength(20, ErrorMessage = "Código da função deve ter no máximo 20 caracteres")]
    public string CodigoFuncao { get; set; } = string.Empty;

    /// <summary>
    /// Nome do botão (obrigatório)
    /// </summary>
    [JsonPropertyName("nome")]
    [Required(ErrorMessage = "Nome do botão é obrigatório")]
    [StringLength(50, ErrorMessage = "Nome deve ter no máximo 50 caracteres")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Descrição do botão (obrigatório)
    /// </summary>
    [JsonPropertyName("descricao")]
    [Required(ErrorMessage = "Descrição é obrigatória")]
    [StringLength(100, ErrorMessage = "Descrição deve ter no máximo 100 caracteres")]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Ação do botão (obrigatório)
    /// </summary>
    [JsonPropertyName("acao")]
    [Required(ErrorMessage = "Ação é obrigatória")]
    [StringLength(20, ErrorMessage = "Ação deve ter no máximo 20 caracteres")]
    public string Acao { get; set; } = string.Empty;
}

/// <summary>
/// DTO para atualização de botões existentes
/// </summary>
public sealed class BotaoUpdateDto
{
    /// <summary>
    /// Descrição do botão (obrigatório)
    /// </summary>
    [JsonPropertyName("descricao")]
    [Required(ErrorMessage = "Descrição é obrigatória")]
    [StringLength(100, ErrorMessage = "Descrição deve ter no máximo 100 caracteres")]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Ação do botão (obrigatório)
    /// </summary>
    [JsonPropertyName("acao")]
    [Required(ErrorMessage = "Ação é obrigatória")]
    [StringLength(20, ErrorMessage = "Ação deve ter no máximo 20 caracteres")]
    public string Acao { get; set; } = string.Empty;
}

/// <summary>
/// DTO para formulários de botão - usado em telas de cadastro
/// </summary>
public sealed class BotaoFormDto
{
    /// <summary>
    /// Código do sistema
    /// </summary>
    public string CodigoSistema { get; set; } = string.Empty;

    /// <summary>
    /// Código da função
    /// </summary>
    public string CodigoFuncao { get; set; } = string.Empty;

    /// <summary>
    /// Nome do botão
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Descrição do botão
    /// </summary>
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Ação do botão
    /// </summary>
    public string Acao { get; set; } = string.Empty;
}

