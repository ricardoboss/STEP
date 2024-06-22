using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// Returns a deep clone of the subject.
/// </summary>
public class CloneFunction : GenericFunction<ExpressionResult>
{
    /// <summary>
    /// The identifier of the <see cref="CloneFunction"/> function.
    /// </summary>
    public const string Identifier = "clone";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(AnyValueType, "subject"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = AnyValueType;

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        ExpressionResult argument1)
    {
        return argument1.DeepClone();
    }
}