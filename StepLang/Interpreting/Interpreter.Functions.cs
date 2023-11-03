using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
    public void Execute(CallStatementNode statementNode)
    {
        var function = CurrentScope.GetVariable(statementNode.Identifier);
        var definition = function.Value.ExpectFunction().Value;

        // TODO: check if function returns anything other than void and abort call

        var result = definition.Invoke(this, statementNode.Arguments);
        if (result is not VoidResult)
            throw new InvalidOperationException("Function call must return void.");
    }

    public ExpressionResult Evaluate(CallExpressionNode expressionNode)
    {
        var function = CurrentScope.GetVariable(expressionNode.Identifier);
        var definition = function.Value.ExpectFunction().Value;
        return definition.Invoke(this, expressionNode.Arguments);
    }

    public ExpressionResult Evaluate(FunctionDefinitionExpressionNode expressionNode)
    {
        throw new NotImplementedException();
    }
}