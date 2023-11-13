using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class RoundFunction : GenericFunction<NumberResult>
{
    public const string Identifier = "round";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[] { new(OnlyNumber, "x") };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyNumber;

    protected override NumberResult Invoke(TokenLocation callLocation, Interpreter interpreter, NumberResult argument1) => Math.Round(argument1.Value);
}