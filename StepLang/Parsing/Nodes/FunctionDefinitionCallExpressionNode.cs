using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes;

public record FunctionDefinitionCallExpressionNode(
	Token OpenParenthesisToken,
	IReadOnlyList<IVariableDeclarationNode> Parameters,
	CodeBlockStatementNode Body,
	IReadOnlyList<ExpressionNode> CallArguments) : ExpressionNode
{
	public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
	{
		return evaluator.Evaluate(this);
	}

	public override TokenLocation Location => OpenParenthesisToken.Location;
}
