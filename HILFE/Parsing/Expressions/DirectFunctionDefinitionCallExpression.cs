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
        if (definition is not { ValueType: "function", Value: FunctionDefinition functionDefinition })
            throw new InvalidResultTypeException("function", definition.ValueType);

        return await functionDefinition.EvaluateAsync(interpreter, args, cancellationToken);
    }

    protected override string DebugDisplay() => $"({definitionExpression})({string.Join(',', args)})";
}