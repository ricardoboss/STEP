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

            if (BreakDepth <= 0)
                continue;

            BreakDepth--;

            break;
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
