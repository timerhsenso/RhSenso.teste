namespace RhS.SharedKernel.Exceptions;

/// <summary>
/// Exceção para violações de regras de negócio
/// </summary>
public class BusinessException : Exception
{
    /// <summary>
    /// Código do erro
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Detalhes adicionais do erro
    /// </summary>
    public object? Details { get; }

    public BusinessException(string message) : base(message)
    {
        ErrorCode = "BUSINESS_ERROR";
    }

    public BusinessException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }

    public BusinessException(string message, string errorCode, object? details) : base(message)
    {
        ErrorCode = errorCode;
        Details = details;
    }

    public BusinessException(string message, Exception innerException) : base(message, innerException)
    {
        ErrorCode = "BUSINESS_ERROR";
    }

    public BusinessException(string message, string errorCode, Exception innerException) : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}

