using System.Text;

namespace RhS.SharedKernel.Security;

/// <summary>
/// Utilitário para codificação e decodificação de chaves compostas
/// </summary>
public static class KeyCodec
{
    /// <summary>
    /// Converte uma string para Base64Url (URL-safe)
    /// </summary>
    /// <param name="input">String a ser codificada</param>
    /// <returns>String codificada em Base64Url</returns>
    public static string ToBase64Url(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        var bytes = Encoding.UTF8.GetBytes(input);
        var base64 = Convert.ToBase64String(bytes);
        
        // Converte para Base64Url (URL-safe)
        return base64.Replace('+', '-')
                    .Replace('/', '_')
                    .TrimEnd('=');
    }

    /// <summary>
    /// Decodifica uma string Base64Url para string original
    /// </summary>
    /// <param name="input">String codificada em Base64Url</param>
    /// <returns>String original decodificada</returns>
    public static string FromBase64Url(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        // Converte de Base64Url para Base64 padrão
        var base64 = input.Replace('-', '+').Replace('_', '/');
        
        // Adiciona padding se necessário
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }

        try
        {
            var bytes = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(bytes);
        }
        catch (FormatException)
        {
            throw new ArgumentException("Invalid Base64Url string", nameof(input));
        }
    }

    /// <summary>
    /// Cria uma chave composta a partir de múltiplos valores
    /// </summary>
    /// <param name="parts">Partes da chave</param>
    /// <returns>Chave composta separada por pipe (|)</returns>
    public static string CreateCompositeKey(params string[] parts)
    {
        if (parts == null || parts.Length == 0)
            return string.Empty;

        return string.Join("|", parts.Where(p => !string.IsNullOrEmpty(p)));
    }

    /// <summary>
    /// Separa uma chave composta em suas partes
    /// </summary>
    /// <param name="compositeKey">Chave composta</param>
    /// <returns>Array com as partes da chave</returns>
    public static string[] SplitCompositeKey(string compositeKey)
    {
        if (string.IsNullOrEmpty(compositeKey))
            return Array.Empty<string>();

        return compositeKey.Split('|', StringSplitOptions.RemoveEmptyEntries);
    }
}

