using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter : IVariableDeclarationVisitor
{
    public void Execute(VariableAssignmentNode statementNode)
    {
        var variable = CurrentScope.GetVariable(statementNode.Identifier);

        var result = statementNode.Expression.EvaluateUsing(this);

        variable.Assign(result);
    }

    public void Execute(VariableDeclarationNode statementNode)
    {
        var validResults = statementNode.Types.Select(t => ResultTypes.FromTypeName(t.Value)).ToList();

        CurrentScope.CreateVariable(statementNode.Identifier, validResults, ExpressionResult.DefaultFor(validResults.First()), nullable: false);
    }

    public void Execute(NullableVariableDeclarationNode statementNode)
    {
        CurrentScope.CreateVariable(statementNode.Identifier, statementNode.Types.Select(t => ResultTypes.FromTypeName(t.Value)).ToList(), NullResult.Instance, nullable: true);
    }

    public void Execute(VariableInitializationNode statementNode)
    {
        CurrentScope.CreateVariable(statementNode.Identifier, statementNode.Types.Select(t => ResultTypes.FromTypeName(t.Value)).ToList(), statementNode.Expression.EvaluateUsing(this), nullable: false);
    }

    public void Execute(NullableVariableInitializationNode statementNode)
    {
        CurrentScope.CreateVariable(statementNode.Identifier, statementNode.Types.Select(t => ResultTypes.FromTypeName(t.Value)).ToList(), statementNode.Expression.EvaluateUsing(this), nullable: true);
    }
}