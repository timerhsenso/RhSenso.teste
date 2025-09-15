namespace RhS.SharedKernel.Exceptions;

/// <summary>
/// Exceção para problemas de autorização
/// </summary>
public class UnauthorizedException : Exception
{
    /// <summary>
    /// Permissão necessária que não foi atendida
    /// </summary>
    public string? RequiredPermission { get; }

    public UnauthorizedException() : base("Acesso negado.")
    {
    }

    public UnauthorizedException(string message) : base(message)
    {
    }

    public UnauthorizedException(string message, string requiredPermission) : base(message)
    {
        RequiredPermission = requiredPermission;
    }

    public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

