namespace RhS.SharedKernel.DTOs;

public class LoginResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Token { get; set; }
    public string? UserName { get; set; }
    public List<PermissionDto> Permissions { get; set; } = new();
}

