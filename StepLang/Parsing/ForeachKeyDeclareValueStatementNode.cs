using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record ForeachKeyDeclareValueStatementNode(
	Token ForeachKeywordToken,
	Token KeyIdentifier,
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
