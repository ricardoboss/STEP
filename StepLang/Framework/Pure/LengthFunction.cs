using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// Returns the length of the given string, list, or map.
/// </summary>
public class LengthFunction : NativeFunction
{
    /// <summary>
    /// The identifier for the <see cref="LengthFunction"/> function.
    /// </summary>
    public const string Identifier = "length";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new[]
    {
        new NativeParameter(new[] { ResultType.Str, ResultType.List, ResultType.Map }, "subject"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = new[] { ResultType.Number };

    /// <inheritdoc />
    public override NumberResult Invoke(TokenLocation callLocation, Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        CheckArgumentCount(callLocation, arguments);

        var subjectResult = arguments.Single().EvaluateUsing(interpreter);

        return subjectResult switch
        {
            StringResult { Value: var str } => str.GraphemeLength(),
            ListResult { Value: var list } => list.Count,
            MapResult { Value: var map } => map.Count,
            _ => throw new InvalidArgumentTypeException(arguments.Single().Location, NativeParameters.Single().Types, subjectResult),
        };
    }
}