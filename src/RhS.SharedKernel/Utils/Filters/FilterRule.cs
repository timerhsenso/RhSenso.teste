
namespace RhS.SharedKernel.Common.Filters;

public sealed class FilterGroup
{
    public string Logic { get; init; } = "and"; // "and" | "or"
    public List<FilterRule> Rules { get; init; } = new();
}

public sealed class FilterRule
{
    public string Field { get; init; } = string.Empty;
    public string Op { get; init; } = "eq"; // eq, ne, lt, le, gt, ge, contains, starts, ends, in
    public string? Value { get; init; }
}
