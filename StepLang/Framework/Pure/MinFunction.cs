using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public class MinFunction : NativeFunction
{
    public const string Identifier = "min";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new[]
    {
        new NativeParameter(new[] { ResultType.Number }, "...values"),
    };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = new[] { ResultType.Number };

    public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        CheckArgumentCount(arguments, 1, int.MaxValue);

        var min = arguments
            .Select(argument => argument.EvaluateUsing(interpreter))
            .OfType<NumberResult>()
            .Min(argument => argument.Value);

        return new NumberResult(min);
    }
}