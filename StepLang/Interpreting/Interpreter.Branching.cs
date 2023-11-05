using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
    public void Execute(IfStatementNode statementNode)
    {
        var result = statementNode.Condition.EvaluateUsing(this);
        if (result.IsTruthy())
            Execute(statementNode.Body);
    }

    public void Execute(IfElseStatementNode statementNode)
    {
        var result = statementNode.Condition.EvaluateUsing(this);
        if (result.IsTruthy())
            Execute(statementNode.Body);
        else
            Execute(statementNode.ElseBody);
    }

    public void Execute(IfElseIfStatementNode statementNode)
    {
        var result = statementNode.Condition.EvaluateUsing(this);
        if (result.IsTruthy())
        {
            Execute(statementNode.Body);
            return;
        }

        var elseResult = statementNode.ElseCondition.EvaluateUsing(this);
        if (elseResult.IsTruthy())
            Execute(statementNode.ElseBody);
    }
}