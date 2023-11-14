using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
    public void Execute(ContinueStatementNode statementNode)
    {
        ContinueDepth++;
    }
}