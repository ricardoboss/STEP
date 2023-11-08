using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
    public void Execute(ReturnExpressionStatementNode statementNode)
    {
        var result = statementNode.Expression.EvaluateUsing(this);

        CurrentScope.SetResult(result);
    }

    public void Execute(ReturnStatementNode statementNode)
    {
        CurrentScope.SetResult(VoidResult.Instance);
    }
}