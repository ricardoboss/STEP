using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
    public void Execute(DiscardStatementNode discardStatementNode)
    {
        _ = discardStatementNode.Expression.EvaluateUsing(this);
    }
}
