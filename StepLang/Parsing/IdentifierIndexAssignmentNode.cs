using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record IdentifierIndexAssignmentNode(Token Identifier, IReadOnlyList<ExpressionNode> IndexExpressions, Token AssignmentToken, ExpressionNode ValueExpression) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }

    public override TokenLocation Location => Identifier.Location;
}