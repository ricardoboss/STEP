using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
    public void Visit(IfStatementNode statementNode)
    {
        var result = statementNode.Condition.EvaluateUsing(this);
        if (result.IsTruthy())
            Execute(statementNode.Body);
    }

    public void Visit(IfElseStatementNode statementNode)
    {
        var result = statementNode.Condition.EvaluateUsing(this);
        if (result.IsTruthy())
            Execute(statementNode.Body);
        else
            Execute(statementNode.ElseBody);
    }

    public void Visit(IfElseIfStatementNode statementNode)
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

    public void Visit(ContinueStatementNode statementNode)
    {
        CurrentScope.SetContinue();
    }

    public void Visit(BreakStatementNode statementNode)
    {
        CurrentScope.SetBreak();
    }
}