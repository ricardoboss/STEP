using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Parsing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
    public void Execute(CallStatementNode statementNode)
    {
        var variable = CurrentScope.GetVariable(statementNode.CallExpression.Identifier);
        if (variable.Value is not FunctionResult function)
            throw new InvalidOperationException("Variable is not a function.");

        // TODO: check if function returns anything other than void and abort call

        var result = function.Value.Invoke(this, statementNode.CallExpression.Arguments);
        if (result is not VoidResult)
            throw new InvalidOperationException("Function call must return void.");
    }

    public ExpressionResult Evaluate(CallExpressionNode expressionNode)
    {
        var variable = CurrentScope.GetVariable(expressionNode.Identifier);
        if (variable.Value is not FunctionResult function)
            throw new InvalidOperationException("Variable is not a function.");

        return function.Value.Invoke(this, expressionNode.Arguments);
    }

    public ExpressionResult Evaluate(FunctionDefinitionExpressionNode expressionNode)
    {
        var definition = new UserDefinedFunctionDefinition(expressionNode.Parameters, expressionNode.Body);

        return new FunctionResult(definition);
    }

    public ExpressionResult Evaluate(FunctionDefinitionCallExpressionNode expressionNode)
    {
        var definition = new UserDefinedFunctionDefinition(expressionNode.Parameters, expressionNode.Body);

        return definition.Invoke(this, expressionNode.CallArguments);
    }
}