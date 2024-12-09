using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record ForeachDeclareKeyValueStatementNode(
	Token ForeachKeywordToken,
	IVariableDeclarationNode KeyDeclaration,
	Token ValueIdentifier,
	ExpressionNode Collection,
	IReadOnlyCollection<StatementNode> Body) : StatementNode
{
	public override void Accept(IStatementVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override TokenLocation Location => ForeachKeywordToken.Location;
}
