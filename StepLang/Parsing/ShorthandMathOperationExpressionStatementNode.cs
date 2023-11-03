using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record ShorthandMathOperationExpressionStatementNode(Token Identifier, Token Operation, ExpressionNode Expression) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }
}