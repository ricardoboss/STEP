using StepLang.Tokenizing;

namespace StepLang.Parsing.Nodes;

public sealed record ForeachDeclareValueStatementNode(
	Token ForeachKeywordToken,
	IVariableDeclarationNode ValueDeclaration,
	ExpressionNode Collection,
	CodeBlockStatementNode Body) : StatementNode
{
	public override void Accept(IStatementVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override TokenLocation Location => ForeachKeywordToken.Location;
}
