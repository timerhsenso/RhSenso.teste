namespace RhS.SEG.Application.DTOs.Botoes;

public class BotaoFormDto
{
    public Guid Id { get; set; }
    public string CdSistema { get; set; } = string.Empty;
    public string CdFuncao { get; set; } = string.Empty;
    public string CdAcao { get; set; } = string.Empty;
    public string DcAcao { get; set; } = string.Empty;
    public string FlAtivo { get; set; } = "S";
}

