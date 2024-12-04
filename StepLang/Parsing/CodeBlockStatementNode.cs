using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record CodeBlockStatementNode(Token OpenCurlyBraceToken, IReadOnlyList<StatementNode> Body) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Visit(this);
    }

    public override TokenLocation Location => OpenCurlyBraceToken.Location;
}
