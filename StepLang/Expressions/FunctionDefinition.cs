using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Parsing.Nodes.VariableDeclarations;
using StepLang.Tokenizing;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.Expressions;

public abstract class FunctionDefinition
{
	[ExcludeFromCodeCoverage]
	private string DebugParamsString => string.Join(", ", Parameters.Select(p =>
	{
		if (p is NullableVariableDeclarationNode nullable)
		{
			return $"{nullable.ResultTypesToString()}{nullable.NullabilityIndicator.Value} {nullable.Identifier.Value}";
		}

		return $"{p.ResultTypesToString()} {p.Identifier.Value}";
	}));

	[ExcludeFromCodeCoverage]
	private string DebugReturnTypeString => string.Join("|", ReturnTypes.Select(r => r.ToString()));

	[ExcludeFromCodeCoverage]
	protected abstract string DebugBodyString { get; }

	/// <inheritdoc />
	[ExcludeFromCodeCoverage]
	public override string ToString()
	{
		var paramStr = DebugParamsString;
		var returnStr = DebugReturnTypeString;
		var bodyStr = DebugBodyString;

		return $"<Function: ({paramStr}): {returnStr} => {{ {bodyStr} }}>";
	}

	public FunctionResult ToResult()
	{
		return new FunctionResult(this);
	}

	public abstract ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
		IReadOnlyList<ExpressionNode> arguments);

	public abstract IReadOnlyList<IVariableDeclarationNode> Parameters { get; }

	protected virtual IEnumerable<ResultType> ReturnTypes { get; } = [ResultType.Void];
}
