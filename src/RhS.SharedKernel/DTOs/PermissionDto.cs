namespace RhS.SharedKernel.DTOs;

public class PermissionDto
{
    public string Sistema { get; set; } = string.Empty;
    public string Funcao { get; set; } = string.Empty;
    public string Acao { get; set; } = string.Empty;
    public bool Permitido { get; set; }
}

