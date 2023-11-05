using StepLang.Tokenizing;

namespace StepLang.Parsing;

public record IncrementStatementNode(Token Identifier) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }
}