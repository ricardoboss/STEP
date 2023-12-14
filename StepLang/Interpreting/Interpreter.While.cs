using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
    public void Execute(WhileStatementNode statementNode)
    {
        while (ShouldLoop())
        {
            PushScope();

            Execute(statementNode.Body);

            var loopScope = PopScope();

            if (BreakDepth > 0)
            {
                BreakDepth--;

                break;
            }

            if (loopScope.TryGetResult(out var resultValue, out var resultLocation))
            {
                CurrentScope.SetResult(resultLocation, resultValue);

                return;
            }
        }

        return;

        bool ShouldLoop()
        {
            var result = statementNode.Condition.EvaluateUsing(this);

            return result is BoolResult { Value: true };
        }
    }
}
