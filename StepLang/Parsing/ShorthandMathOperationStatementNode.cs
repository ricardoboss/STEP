using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record ShorthandMathOperationStatementNode(Token Identifier, Token Operation) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }

    public override TokenLocation Location => Identifier.Location;
}