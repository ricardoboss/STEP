using StepLang.Tokenizing;

namespace StepLang.Parsing;

public record ReturnStatementNode(Token ReturnKeyword) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }

    public override TokenLocation Location => ReturnKeyword.Location;
}