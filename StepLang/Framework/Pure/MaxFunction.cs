using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// Returns the largest number from the given numbers.
/// </summary>
public class MaxFunction : NativeFunction
{
    /// <summary>
    /// The identifier for the <see cref="MaxFunction"/> function.
    /// </summary>
    public const string Identifier = "max";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new[]
    {
        new NativeParameter(new[] { ResultType.Number }, "...values"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = new[] { ResultType.Number };

    /// <inheritdoc />
    public override NumberResult Invoke(TokenLocation callLocation, Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        CheckArgumentCount(callLocation, arguments, 1, int.MaxValue);

        return arguments
            .Select(argument => argument.EvaluateUsing(interpreter))
            .OfType<NumberResult>()
            .Max(argument => argument.Value);
    }
}