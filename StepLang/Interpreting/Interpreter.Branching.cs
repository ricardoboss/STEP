using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
    public void Execute(IfStatementNode statementNode)
    {
        foreach (var (condition, body) in statementNode.ConditionBodyMap)
        {
            var result = condition.EvaluateUsing(this);
            if (!result.IsTruthy())
                continue;

            Execute(body);

            break;
        }
    }

    public void Execute(ContinueStatementNode statementNode)
    {
        CurrentScope.SetContinue();
    }

    public void Execute(BreakStatementNode statementNode)
    {
        CurrentScope.SetBreak();
    }
}