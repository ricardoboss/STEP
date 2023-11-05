using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
    public void Execute(ReturnStatementNode statementNode)
    {
        var result = statementNode.Expression.EvaluateUsing(this);

        CurrentScope.SetResult(result);
    }
}