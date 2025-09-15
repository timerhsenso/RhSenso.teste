
using System.Linq.Expressions;
using System.Reflection;
using RhS.SharedKernel.Common.Filters;

namespace RhS.Web.Infrastructure.EF;

public static class DynamicFilterExtensions
{
    public static IQueryable<T> ApplyFilterGroup<T>(this IQueryable<T> query, FilterGroup? group)
    {
        if (group == null || group.Rules.Count == 0) return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression? combined = null;

        foreach (var rule in group.Rules)
        {
            var expr = BuildRuleExpression<T>(parameter, rule);
            if (expr == null) continue;

            combined = combined == null
                ? expr
                : (group.Logic.Equals("or", StringComparison.OrdinalIgnoreCase)
                    ? Expression.OrElse(combined, expr)
                    : Expression.AndAlso(combined, expr));
        }

        if (combined == null) return query;
        var lambda = Expression.Lambda<Func<T, bool>>(combined, parameter);
        return query.Where(lambda);
    }

    private static Expression? BuildRuleExpression<T>(ParameterExpression param, FilterRule rule)
    {
        var prop = typeof(T).GetProperty(rule.Field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (prop == null) return null;

        var left = Expression.Property(param, prop);
        var constant = ConvertValue(rule.Value, prop.PropertyType);
        if (constant == null && rule.Op is not ("eq" or "ne")) return null;

        return rule.Op.ToLowerInvariant() switch
        {
            "eq" => Expression.Equal(left, Expression.Constant(constant, prop.PropertyType)),
            "ne" => Expression.NotEqual(left, Expression.Constant(constant, prop.PropertyType)),
            "lt" => Expression.LessThan(left, Expression.Constant(constant, prop.PropertyType)),
            "le" => Expression.LessThanOrEqual(left, Expression.Constant(constant, prop.PropertyType)),
            "gt" => Expression.GreaterThan(left, Expression.Constant(constant, prop.PropertyType)),
            "ge" => Expression.GreaterThanOrEqual(left, Expression.Constant(constant, prop.PropertyType)),
            "contains" => CallString(left, "Contains", constant),
            "starts" => CallString(left, "StartsWith", constant),
            "ends" => CallString(left, "EndsWith", constant),
            "in" => BuildIn(left, prop.PropertyType, rule.Value),
            _ => null
        };
    }

    private static object? ConvertValue(string? value, Type type)
    {
        if (value is null) return type.IsValueType ? Activator.CreateInstance(type) : null;
        if (type == typeof(string)) return value;
        if (type.IsEnum) return Enum.Parse(type, value, true);
        if (type == typeof(Guid) || type == typeof(Guid?)) return Guid.Parse(value);
        if (Nullable.GetUnderlyingType(type) is Type nt) return Convert.ChangeType(value, nt);
        return Convert.ChangeType(value, type);
    }

    private static Expression? CallString(MemberExpression left, string method, object? constant)
    {
        if (left.Type != typeof(string)) return null;
        return Expression.Call(left, typeof(string).GetMethod(method, new[] { typeof(string) })!, Expression.Constant(constant, typeof(string)));
    }

    private static Expression? BuildIn(MemberExpression left, Type t, string? csv)
    {
        if (string.IsNullOrWhiteSpace(csv)) return null;
        var items = csv.Split(',').Select(s => s.Trim()).Where(s => s.Length > 0).ToArray();
        var listType = typeof(List<>).MakeGenericType(t);
        var list = Activator.CreateInstance(listType)!;
        var add = listType.GetMethod("Add")!;
        foreach (var s in items) add.Invoke(list, new[] { ConvertValue(s, t) });

        var contains = listType.GetMethod("Contains", new[] { t })!;
        return Expression.Call(Expression.Constant(list), contains, left);
    }
}
