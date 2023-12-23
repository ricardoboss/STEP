using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// Returns whether a value is contained in a list, map, or string.
/// </summary>
public class ContainsFunction : GenericFunction<ExpressionResult, ExpressionResult>
{
    /// <summary>
    /// The identifier of the <see cref="ContainsFunction"/> function.
    /// </summary>
    public const string Identifier = "contains";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[] {
        new(new[] { ResultType.List, ResultType.Map, ResultType.Str }, "subject"),
        new(AnyValueType, "value"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyBool;

    /// <inheritdoc />
    protected override BoolResult Invoke(TokenLocation callLocation, Interpreter interpreter, ExpressionResult argument1, ExpressionResult argument2)
    {
        var result = IndexOfFunction.GetResult(argument1, argument2);

        return result switch
        {
            NumberResult { Value: >= 0 } => true,
            StringResult => true,
            _ => false,
        };
    }
}