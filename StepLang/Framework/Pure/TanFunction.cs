using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Pure;

public class TanFunction : GenericOneParameterFunction
{
    public const string Identifier = "tan";

    protected override IEnumerable<NativeParameter> NativeParameters => new NativeParameter[] { new(new[] { ResultType.Number }, "x") };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = new[] { ResultType.Number };

    protected override ExpressionResult Invoke(Interpreter interpreter, NumberResult argument)
    {
        return (NumberResult)Math.Tan(argument);
    }
}