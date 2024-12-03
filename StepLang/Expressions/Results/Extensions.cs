namespace StepLang.Expressions.Results;

public static class Extensions
{
    public static ListResult ToListResult(this IEnumerable<ExpressionResult> value)
    {
        return new ListResult(value.ToList());
    }

    public static ListResult ToListResult<T>(this IEnumerable<T> value, Func<T, ExpressionResult> converter)
    {
        return value.Select(converter).ToListResult();
    }
}