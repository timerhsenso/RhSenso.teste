using System.Text.Json.Serialization;

namespace RhS.SharedKernel.Common.DataTables;

/// <summary>
/// Envelope compatível com DataTables server-side processing
/// Saída em camelCase por padrão do .NET: draw, recordsTotal, recordsFiltered, data, error
/// </summary>
/// <typeparam name="T">Tipo dos dados retornados</typeparam>
public sealed class DataTableResponse<T>
{
    /// <summary>
    /// Contador de requisições (echo) para sincronização com o cliente
    /// </summary>
    [JsonPropertyName("draw")]
    public int Draw { get; init; }

    /// <summary>
    /// Total de registros na base de dados (sem filtros)
    /// </summary>
    [JsonPropertyName("recordsTotal")]
    public int RecordsTotal { get; init; }

    /// <summary>
    /// Total de registros após aplicar filtros
    /// </summary>
    [JsonPropertyName("recordsFiltered")]
    public int RecordsFiltered { get; init; }

    /// <summary>
    /// Dados da página atual
    /// </summary>
    [JsonPropertyName("data")]
    public IReadOnlyList<T> Data { get; init; }

    /// <summary>
    /// Mensagem de erro, se houver
    /// </summary>
    [JsonPropertyName("error")]
    public string? Error { get; init; }

    /// <summary>
    /// Construtor para resposta de sucesso
    /// </summary>
    /// <param name="draw">Contador de requisições</param>
    /// <param name="recordsTotal">Total de registros</param>
    /// <param name="recordsFiltered">Total filtrado</param>
    /// <param name="data">Dados da página</param>
    [JsonConstructor]
    public DataTableResponse(int draw, int recordsTotal, int recordsFiltered, IReadOnlyList<T> data)
    {
        Draw = draw;
        RecordsTotal = recordsTotal;
        RecordsFiltered = recordsFiltered;
        Data = data;
        Error = null;
    }

    /// <summary>
    /// Construtor para resposta com erro
    /// </summary>
    /// <param name="draw">Contador de requisições</param>
    /// <param name="error">Mensagem de erro</param>
    public DataTableResponse(int draw, string error)
    {
        Draw = draw;
        RecordsTotal = 0;
        RecordsFiltered = 0;
        Data = Array.Empty<T>();
        Error = error;
    }

    /// <summary>
    /// Cria uma resposta de sucesso
    /// </summary>
    /// <param name="draw">Contador de requisições</param>
    /// <param name="recordsTotal">Total de registros</param>
    /// <param name="recordsFiltered">Total filtrado</param>
    /// <param name="data">Dados da página</param>
    /// <returns>Resposta de sucesso</returns>
    public static DataTableResponse<T> Success(int draw, int recordsTotal, int recordsFiltered, IReadOnlyList<T> data)
    {
        return new DataTableResponse<T>(draw, recordsTotal, recordsFiltered, data);
    }

    /// <summary>
    /// Cria uma resposta de erro
    /// </summary>
    /// <param name="draw">Contador de requisições</param>
    /// <param name="error">Mensagem de erro</param>
    /// <returns>Resposta de erro</returns>
    public static DataTableResponse<T> CreateError(int draw, string error)
    {
        return new DataTableResponse<T>(draw, error);
    }
}

/// <summary>
/// Parâmetros de requisição do DataTables
/// </summary>
public sealed class DataTableRequest
{
    /// <summary>
    /// Contador de requisições (echo)
    /// </summary>
    [JsonPropertyName("draw")]
    public int Draw { get; set; }

    /// <summary>
    /// Índice do primeiro registro a ser retornado
    /// </summary>
    [JsonPropertyName("start")]
    public int Start { get; set; }

    /// <summary>
    /// Número de registros por página
    /// </summary>
    [JsonPropertyName("length")]
    public int Length { get; set; }

    /// <summary>
    /// Termo de busca global
    /// </summary>
    [JsonPropertyName("search")]
    public DataTableSearch? Search { get; set; }

    /// <summary>
    /// Configurações de ordenação
    /// </summary>
    [JsonPropertyName("order")]
    public IList<DataTableOrder>? Order { get; set; }

    /// <summary>
    /// Configurações de colunas
    /// </summary>
    [JsonPropertyName("columns")]
    public IList<DataTableColumn>? Columns { get; set; }
}

/// <summary>
/// Configuração de busca do DataTables
/// </summary>
public sealed class DataTableSearch
{
    /// <summary>
    /// Termo de busca
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Se a busca é por regex
    /// </summary>
    [JsonPropertyName("regex")]
    public bool Regex { get; set; }
}

/// <summary>
/// Configuração de ordenação do DataTables
/// </summary>
public sealed class DataTableOrder
{
    /// <summary>
    /// Índice da coluna
    /// </summary>
    [JsonPropertyName("column")]
    public int Column { get; set; }

    /// <summary>
    /// Direção da ordenação (asc/desc)
    /// </summary>
    [JsonPropertyName("dir")]
    public string Dir { get; set; } = "asc";
}

/// <summary>
/// Configuração de coluna do DataTables
/// </summary>
public sealed class DataTableColumn
{
    /// <summary>
    /// Nome da coluna
    /// </summary>
    [JsonPropertyName("data")]
    public string Data { get; set; } = string.Empty;

    /// <summary>
    /// Nome da coluna (alternativo)
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Se a coluna é pesquisável
    /// </summary>
    [JsonPropertyName("searchable")]
    public bool Searchable { get; set; }

    /// <summary>
    /// Se a coluna é ordenável
    /// </summary>
    [JsonPropertyName("orderable")]
    public bool Orderable { get; set; }

    /// <summary>
    /// Configuração de busca da coluna
    /// </summary>
    [JsonPropertyName("search")]
    public DataTableSearch? Search { get; set; }
}

