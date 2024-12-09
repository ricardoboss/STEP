using StepLang.Tokenizing;

namespace StepLang.Parsing;

public record IncrementStatementNode(Token Identifier) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Visit(this);
    }

    public override TokenLocation Location => Identifier.Location;
}