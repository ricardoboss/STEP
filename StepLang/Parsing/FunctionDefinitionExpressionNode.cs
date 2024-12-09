using StepLang.Expressions.Results;
using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record FunctionDefinitionExpressionNode(
	Token OpenParenthesisToken,
	IReadOnlyList<IVariableDeclarationNode> Parameters,
	IReadOnlyList<StatementNode> Body) : ExpressionNode
{
	public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
	{
		return evaluator.Evaluate(this);
	}

	public override TokenLocation Location => OpenParenthesisToken.Location;
}
