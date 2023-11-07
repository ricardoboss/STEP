using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Pure;

public class FloorFunction : GenericFunction<NumberResult>
{
    public const string Identifier = "floor";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[] { new(OnlyNumber, "x") };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyNumber;

    protected override NumberResult Invoke(Interpreter interpreter, NumberResult argument1) => Math.Floor(argument1.Value);
}