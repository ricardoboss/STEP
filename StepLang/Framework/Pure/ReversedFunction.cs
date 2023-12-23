using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// Reverses a list or string.
/// </summary>
public class ReversedFunction : GenericFunction<ExpressionResult>
{
    /// <summary>
    /// The identifier of the <see cref="ReversedFunction"/> function.
    /// </summary>
    public const string Identifier = "reversed";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(new[] { ResultType.List, ResultType.Str }, "subject"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = new[] { ResultType.List, ResultType.Str };

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        ExpressionResult argument1)
    {
        return argument1 switch
        {
            ListResult list => new ListResult(list.DeepClone().Value.Reverse().ToList()),
            StringResult str => new StringResult(str.Value.ReverseGraphemes()),
            _ => throw new InvalidResultTypeException(callLocation, argument1, ResultType.List, ResultType.Str),
        };
    }
}
