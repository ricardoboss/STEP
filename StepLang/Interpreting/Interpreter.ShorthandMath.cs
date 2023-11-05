using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
    public void Execute(IncrementStatementNode incrementStatementNode)
    {
        var variable = CurrentScope.GetVariable(incrementStatementNode.Identifier);
        var value = variable.Value;
        if (value is not NumberResult number)
            throw new IncompatibleExpressionOperandsException(value, "increment");

        variable.Assign(number + 1);
    }

    public void Execute(DecrementStatementNode incrementStatementNode)
    {
        var variable = CurrentScope.GetVariable(incrementStatementNode.Identifier);
        var value = variable.Value;
        if (value is not NumberResult number)
            throw new IncompatibleExpressionOperandsException(value, "decrement");

        variable.Assign(number - 1);
    }

    public void Execute(ShorthandMathOperationExpressionStatementNode statementNode)
    {
        throw new NotImplementedException();
    }

    public void Execute(ShorthandMathOperationStatementNode statementNode)
    {
        throw new NotImplementedException();
    }
}
