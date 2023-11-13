using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class CloneFunction : GenericFunction<ExpressionResult>
{
    public const string Identifier = "clone";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(AnyValueType, "subject"),
    };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = AnyValueType;

    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        ExpressionResult argument1)
    {
        return argument1.DeepClone();
    }
}