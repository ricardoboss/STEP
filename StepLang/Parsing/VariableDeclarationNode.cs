using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record VariableDeclarationNode(IReadOnlyCollection<Token> Types, Token Identifier) : StatementNode, IVariableDeclarationNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }

    public void Accept(IVariableDeclarationVisitor visitor)
    {
        visitor.Execute(this);
    }
}