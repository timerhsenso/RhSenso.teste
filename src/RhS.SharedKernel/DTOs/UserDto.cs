namespace RhS.SharedKernel.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string CdUsuario { get; set; } = string.Empty;
    public string DcUsuario { get; set; } = string.Empty;
    public string? SenhaUser { get; set; }
    public string? EmailUsuario { get; set; }
    public string FlAtivo { get; set; } = "S";
    public int NoUser { get; set; }
}

