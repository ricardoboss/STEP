using HILFE.Interpreting;

namespace HILFE.Parsing.Expressions;

public class DirectFunctionDefinitionCallExpression : Expression
{
    private readonly FunctionDefinitionExpression definitionExpression;
    private readonly IReadOnlyList<Expression> args;

    public DirectFunctionDefinitionCallExpression(FunctionDefinitionExpression definitionExpression, IReadOnlyList<Expression> args)
    {
        this.definitionExpression = definitionExpression;
        this.args = args;
    }

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var definition = await definitionExpression.EvaluateAsync(interpreter, cancellationToken);
        if (definition.ValueType != "function" || definition.Value is not FunctionDefinition functionDefinition)
            throw new InterpreterException($"Expected a function definition, but got {definition}");
        
        return await functionDefinition.EvaluateAsync(interpreter, args, cancellationToken);
    }

    protected override string DebugDisplay() => $"({definitionExpression})({string.Join(',', args)})";
}