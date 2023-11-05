using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record IdentifierIndexAssignmentNode(Token Identifier, ExpressionNode IndexExpression, ExpressionNode ValueExpression) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }

    public override TokenLocation Location => Identifier.Location;
}