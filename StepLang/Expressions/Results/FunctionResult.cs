using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Parsing.Nodes.VariableDeclarations;
using StepLang.Tokenizing;

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

	public override FunctionResult DeepClone()
	{
		return new FunctionResult(Value);
	}

	private class VoidFunctionDefinition : FunctionDefinition
	{
		protected override string DebugBodyString => "";

		public override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
			IReadOnlyList<ExpressionNode> arguments)
		{
			return VoidResult.Instance;
		}

		public override IReadOnlyList<IVariableDeclarationNode> Parameters => Array.Empty<IVariableDeclarationNode>();

		protected override IEnumerable<ResultType> ReturnTypes => [ResultType.Void];
	}
}
