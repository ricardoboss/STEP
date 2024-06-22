using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Expressions.Results;

/// <summary>
/// Represents the result of a function definition and can be called or saved in a variable.
/// </summary>
public class FunctionResult : ValueExpressionResult<FunctionDefinition>
{
    /// <summary>
    /// <para>
    /// The default value for <see cref="ResultType"/>.<see cref="ResultType.Function"/>.
    /// </para>
    /// <para>
    /// This function takes no arguments and returns no value (i.e. <see cref="VoidResult"/>.<see cref="VoidResult.Instance"/>).
    /// </para>
    /// </summary>
    public static FunctionResult VoidFunction => new(new VoidFunctionDefinition());

    /// <summary>
    /// Creates a new <see cref="FunctionResult"/> with the given <see cref="FunctionDefinition"/>.
    /// </summary>
    /// <param name="value">The <see cref="FunctionDefinition"/> to use.</param>
    public FunctionResult(FunctionDefinition value) : base(ResultType.Function, value)
    {
    }

    /// <inheritdoc />
    protected override bool EqualsInternal(ExpressionResult other)
    {
        return other is FunctionResult functionResult && ReferenceEquals(Value, functionResult.Value);
    }

    /// <inheritdoc />
    public override FunctionResult DeepClone() => new(Value);

    private class VoidFunctionDefinition : FunctionDefinition
    {
        protected override string DebugBodyString => "";

        public override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
            IReadOnlyList<ExpressionNode> arguments) => VoidResult.Instance;

        public override IReadOnlyCollection<IVariableDeclarationNode> Parameters => Array.Empty<IVariableDeclarationNode>();

        protected override IEnumerable<ResultType> ReturnTypes => new[] { ResultType.Void };
    }
}