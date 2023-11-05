using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter : IVariableDeclarationEvaluator
{
    public void Execute(VariableDeclarationStatement statementNode)
    {
        _ = statementNode.Declaration.EvaluateUsing(this);
    }

    public Variable Execute(VariableDeclarationNode statementNode)
    {
        var validResults = statementNode.Types.Select(t => ResultTypes.FromTypeName(t.Value)).ToList();

        return CurrentScope.CreateVariable(statementNode.Identifier, validResults, ExpressionResult.DefaultFor(validResults.First()), nullable: false);
    }

    public Variable Execute(NullableVariableDeclarationNode statementNode)
    {
        return CurrentScope.CreateVariable(statementNode.Identifier, statementNode.Types.Select(t => ResultTypes.FromTypeName(t.Value)).ToList(), NullResult.Instance, nullable: true);
    }

    public Variable Execute(VariableInitializationNode statementNode)
    {
        return CurrentScope.CreateVariable(statementNode.Identifier, statementNode.Types.Select(t => ResultTypes.FromTypeName(t.Value)).ToList(), statementNode.Expression.EvaluateUsing(this), nullable: false);
    }

    public Variable Execute(NullableVariableInitializationNode statementNode)
    {
        return CurrentScope.CreateVariable(statementNode.Identifier, statementNode.Types.Select(t => ResultTypes.FromTypeName(t.Value)).ToList(), statementNode.Expression.EvaluateUsing(this), nullable: true);
    }

    public void Execute(VariableAssignmentNode statementNode)
    {
        var variable = CurrentScope.GetVariable(statementNode.Identifier);

        var result = statementNode.Expression.EvaluateUsing(this);

        variable.Assign(result);
    }
}