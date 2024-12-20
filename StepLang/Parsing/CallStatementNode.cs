using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record CallStatementNode(CallExpressionNode CallExpression) : StatementNode
{
	public override void Accept(IStatementVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override TokenLocation Location => CallExpression.Location;
}
