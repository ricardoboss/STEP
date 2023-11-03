using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record VariableAssignmentNode(Token Identifier, ExpressionNode Expression) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }
}