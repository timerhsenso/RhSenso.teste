using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RhS.SEG.Application.DTOs.Sistemas
{
    /// <summary>
    /// DTO para listagem de sistemas - usado em grids e consultas
    /// </summary>
    public sealed record SistemaListDto(
        [property: JsonPropertyName("codigo")] string Codigo,
        [property: JsonPropertyName("descricao")] string Descricao
    );

    /// <summary>
    /// DTO para criação de novos sistemas
    /// </summary>
    public sealed class SistemaCreateDto
    {
        [JsonPropertyName("codigo")]
        [Required(ErrorMessage = "Código do sistema é obrigatório")]
        [StringLength(10, ErrorMessage = "Código deve ter no máximo 10 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [JsonPropertyName("descricao")]
        [Required(ErrorMessage = "Descrição do sistema é obrigatória")]
        [StringLength(100, ErrorMessage = "Descrição deve ter no máximo 100 caracteres")]
        public string Descricao { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para atualização de sistemas existentes
    /// </summary>
    public sealed class SistemaUpdateDto
    {
        [JsonPropertyName("descricao")]
        [Required(ErrorMessage = "Descrição do sistema é obrigatória")]
        [StringLength(100, ErrorMessage = "Descrição deve ter no máximo 100 caracteres")]
        public string Descricao { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para formulários de sistema - usado em telas de cadastro
    /// </summary>
    public sealed class SistemaFormDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
    }
}
