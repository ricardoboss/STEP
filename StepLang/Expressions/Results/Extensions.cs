namespace StepLang.Expressions.Results;

public static class Extensions
{
    public static ListResult ToListResult(this IEnumerable<ExpressionResult> value)
    {
        return new ListResult(value.ToList());
    }

    public static ListResult ToListOfMapsResult(this IEnumerable<Dictionary<string, ExpressionResult>> value)
    {
        return value.Select(d => d.ToMapResult()).ToListResult();
    }

    public static ListResult ToListResult<T>(this IEnumerable<T> value, Func<T, ExpressionResult> converter)
    {
        return value.Select(converter).ToListResult();
    }

    public static MapResult ToMapResult(this Dictionary<string, ExpressionResult> value)
    {
        return new MapResult(value);
    }

    public static MapResult ToMapResult<TResult>(this Dictionary<string, TResult> value)
        where TResult : ExpressionResult
    {
        return value.ToDictionary(kvp => kvp.Key, ExpressionResult (kvp) => kvp.Value).ToMapResult();
    }

    public static MapResult ToNestedMapsResult(
        this Dictionary<string, Dictionary<string, ExpressionResult>> value)
    {
        return value.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.ToDictionary(
                        kvp2 => kvp2.Key,
                        kvp2 => kvp2.Value
                    )
                    .ToMapResult()
            )
            .ToMapResult();
    }

    public static StringResult ToStringResult(this string value)
    {
        return new StringResult(value);
    }

    public static NumberResult ToNumberResult(this int value)
    {
        return new NumberResult(value);
    }

    public static NumberResult ToNumberResult(this double value)
    {
        return new NumberResult(value);
    }
}