using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter : IVariableDeclarationEvaluator
{
    public void Execute(VariableDeclarationStatementNode statementNode)
    {
        _ = statementNode.Declaration.EvaluateUsing(this);
    }

    public Variable Execute(VariableDeclarationNode statementNode)
    {
        var validResults = statementNode.GetResultTypes().ToList();

        return CurrentScope.CreateVariable(statementNode.Location, statementNode.Identifier, validResults, ExpressionResult.DefaultFor(validResults.First()), nullable: false);
    }

    public Variable Execute(NullableVariableDeclarationNode statementNode)
    {
        var validResults = statementNode.GetResultTypes().ToList();

        return CurrentScope.CreateVariable(statementNode.Location, statementNode.Identifier, validResults, NullResult.Instance, nullable: true);
    }

    public Variable Execute(VariableInitializationNode statementNode)
    {
        var validResults = statementNode.GetResultTypes().ToList();

        return CurrentScope.CreateVariable(statementNode.Location, statementNode.Identifier, validResults, statementNode.Expression.EvaluateUsing(this), nullable: false);
    }

    public Variable Execute(NullableVariableInitializationNode statementNode)
    {
        var validResults = statementNode.GetResultTypes().ToList();

        return CurrentScope.CreateVariable(statementNode.Location, statementNode.Identifier, validResults, statementNode.Expression.EvaluateUsing(this), nullable: true);
    }

    public void Execute(VariableAssignmentNode statementNode)
    {
        var variable = CurrentScope.GetVariable(statementNode.Identifier);

        var result = statementNode.Expression.EvaluateUsing(this);

        variable.Assign(statementNode.Location, result);
    }
}