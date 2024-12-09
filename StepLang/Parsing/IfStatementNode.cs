using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record IfStatementNode(
	Token IfKeywordToken,
	LinkedList<(ExpressionNode, CodeBlockStatementNode)> ConditionBodyMap) : StatementNode
{
	public override void Accept(IStatementVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override TokenLocation Location => IfKeywordToken.Location;
}
