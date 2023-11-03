namespace StepLang.Parsing;

public abstract record StatementNode : IVisitableNode<IStatementVisitor>
{
    public abstract void Accept(IStatementVisitor visitor);
}
