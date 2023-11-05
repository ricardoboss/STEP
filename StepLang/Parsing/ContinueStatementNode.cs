using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record ContinueStatementNode(Token Token) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }

    public override TokenLocation Location => Token.Location;
}