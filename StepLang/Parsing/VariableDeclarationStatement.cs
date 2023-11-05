using StepLang.Tokenizing;

namespace StepLang.Parsing;

public record VariableDeclarationStatement(IVariableDeclarationNode Declaration) : StatementNode
{
    public override void Accept(IStatementVisitor visitor)
    {
        visitor.Execute(this);
    }

    public override TokenLocation Location => Declaration.Location;
}