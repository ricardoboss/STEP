using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
    public void Execute(WhileStatementNode statementNode)
    {
        while (ShouldLoop())
        {
            var loopScope = PushScope();

            foreach (var statement in statementNode.Body)
            {
                Execute(statement);

                if (loopScope.ShouldReturn() || loopScope.ShouldBreak() || loopScope.ShouldContinue())
                    break;
            }

            _ = PopScope();

            // handle returns to parent scope
            if (loopScope.TryGetResult(out var resultValue, out var resultLocation))
            {
                CurrentScope.SetResult(resultLocation, resultValue);

                return;
            }

            // break out of loop
            if (loopScope.ShouldBreak())
                return;

            // continue is implicitly handled
        }

        return;

        bool ShouldLoop()
        {
            var result = statementNode.Condition.EvaluateUsing(this);

            return result.IsTruthy();
        }
    }
}
