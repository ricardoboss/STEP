using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// Returns the index of the given value in the given subject.
/// </summary>
public class IndexOfFunction : GenericFunction<ExpressionResult, ExpressionResult>
{
    /// <summary>
    /// The identifier of the <see cref="IndexOfFunction"/> function.
    /// </summary>
    public const string Identifier = "indexOf";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(new[] { ResultType.List, ResultType.Map, ResultType.Str }, "subject"),
        new(AnyValueType, "value"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = new[]
    {
        ResultType.Null, ResultType.Number, ResultType.Str,
    };

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        ExpressionResult argument1, ExpressionResult argument2)
    {
        return GetResult(argument1, argument2);
    }

    internal static ExpressionResult GetResult(ExpressionResult subject, ExpressionResult value)
    {
        return subject switch
        {
            ListResult list => GetListIndex(list, value),
            MapResult map => GetMapKey(map, value),
            StringResult haystack when value is StringResult needle => GetStringIndex(haystack, needle),
            _ => NullResult.Instance,
        };
    }

    private static NumberResult GetListIndex(ListResult list, ExpressionResult value)
    {
        return list.Value.IndexOf(value);
    }

    private static NumberResult GetStringIndex(StringResult haystack, StringResult needle)
    {
        return haystack.Value.GraphemeIndexOf(needle.Value);
    }

    private static ExpressionResult GetMapKey(MapResult map, ExpressionResult value)
    {
        var pair = map.Value.FirstOrDefault(x => x.Value.Equals(value));

        return pair.Equals(default(KeyValuePair<string, ExpressionResult>)) ?
            NullResult.Instance :
            new StringResult(pair.Key);
    }
}