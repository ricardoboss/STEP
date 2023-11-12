using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
    public void Execute(IncrementStatementNode statementNode)
    {
        var variable = CurrentScope.GetVariable(statementNode.Identifier);
        var value = variable.Value;
        if (value is not NumberResult number)
            throw new IncompatibleExpressionOperandsException(statementNode.Location, value, "increment");

        variable.Assign(statementNode.Location, number + 1);
    }

    public void Execute(DecrementStatementNode statementNode)
    {
        var variable = CurrentScope.GetVariable(statementNode.Identifier);
        var value = variable.Value;
        if (value is not NumberResult number)
            throw new IncompatibleExpressionOperandsException(statementNode.Location, value, "decrement");

        variable.Assign(statementNode.Location, number - 1);
    }
}
