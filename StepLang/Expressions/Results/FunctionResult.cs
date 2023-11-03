using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Expressions.Results;

public class FunctionResult : ValueExpressionResult<FunctionDefinition>
{
    public static FunctionResult VoidFunction => new(new VoidFunctionDefinition());

    /// <inheritdoc />
    public FunctionResult(FunctionDefinition value) : base(ResultType.Function, value)
    {
    }

    protected override bool EqualsInternal(ExpressionResult other)
    {
        return other is FunctionResult functionResult && ReferenceEquals(Value, functionResult.Value);
    }

    public override FunctionResult DeepClone() => new(Value);

    private class VoidFunctionDefinition : FunctionDefinition
    {
        protected override string DebugBodyString => "";

        public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments) => VoidResult.Instance;

        public override IReadOnlyCollection<IVariableDeclarationNode> Parameters => Array.Empty<IVariableDeclarationNode>();

        public override IEnumerable<ResultType> ReturnTypes => new[] { ResultType.Void };
    }
}