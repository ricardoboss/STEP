using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Parsing.Nodes.VariableDeclarations;
using StepLang.Tokenizing;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace StepLang.Expressions;

public abstract class FunctionDefinition
{
	[ExcludeFromCodeCoverage]
	private string DebugParamsString
	{
		get
		{
			var sb = new StringBuilder();
			return string.Join(", ", Parameters.Select(p =>
			{
				sb.Clear();

				sb.Append(p.ResultTypesToString());

				if (p is NullableVariableDeclarationNode nullable)
					sb.Append(nullable.NullabilityIndicator.Value);

				sb.Append(' ');
				sb.Append(p.Identifier.Value);

				if (p is IVariableInitializationNode { Expression: { } expression })
				{
					sb.Append(" = ");

					if (expression is LiteralExpressionNode { Literal: { } literal })
						sb.Append(literal.Value);
					else
					{
						sb.Append('{');
						sb.Append(expression.GetType());
						sb.Append('}');
					}
				}

				return sb.ToString();
			}));
		}
	}

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

	public ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		IReadOnlyList<ExpressionResult> arguments) => Invoke(callLocation, interpreter,
		arguments.Select(a => a.ToExpressionNode()).ToList());

	public abstract ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		IReadOnlyList<IExpressionNode> arguments);

	public abstract IReadOnlyList<IVariableDeclarationNode> Parameters { get; }

	protected virtual IEnumerable<ResultType> ReturnTypes { get; } = [ResultType.Void];
}
