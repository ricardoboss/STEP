using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record VariableAssignmentNode(TokenLocation AssignmentLocation, Token Identifier, ExpressionNode Expression) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Visit(this);
    }

    public override TokenLocation Location => AssignmentLocation;
}