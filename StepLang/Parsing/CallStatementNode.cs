using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record CallStatementNode(Token Identifier, IReadOnlyList<ExpressionNode> Arguments) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }
}