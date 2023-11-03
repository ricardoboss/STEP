using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Pure;

public class TanFunction : GenericFunction<NumberResult>
{
    public const string Identifier = "tan";

    protected override IEnumerable<(ResultType[] types, string identifier)> NativeParameters => new[] { (new[] { ResultType.Number }, "x") };

    public override IEnumerable<ResultType> ReturnTypes => new[] { ResultType.Number };

    protected override ExpressionResult Invoke(Interpreter interpreter, NumberResult argument)
    {
        return new NumberResult(Math.Tan(argument.Value));
    }
}