using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
    public void Execute(WhileStatementNode statementNode)
    {
        PushScope();

        while (ShouldLoop())
        {
            Execute(statementNode.Body);

            if (BreakDepth > 0)
            {
                BreakDepth--;

                break;
            }

            if (CurrentScope.TryGetResult(out var resultValue, out var resultLocation))
            {
                PopScope();

                CurrentScope.SetResult(resultLocation, resultValue);

                return;
            }
        }

        PopScope();

        return;

        bool ShouldLoop()
        {
            var result = statementNode.Condition.EvaluateUsing(this);

            return result is BoolResult { Value: true };
        }
    }
}
