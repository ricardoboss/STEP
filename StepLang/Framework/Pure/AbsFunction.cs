using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Pure;

public class AbsFunction : GenericFunction<NumberResult>
{
    public const string Identifier = "abs";
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[] { new(OnlyNumber, "x") };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyNumber;

    protected override NumberResult Invoke(Interpreter interpreter, NumberResult argument1) => Math.Abs(argument1);
}