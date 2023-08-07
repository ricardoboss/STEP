using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Expressions;

public class DirectFunctionDefinitionCallExpression : BaseFunctionCallExpression
{
    private readonly FunctionDefinitionExpression definitionExpression;

    public DirectFunctionDefinitionCallExpression(FunctionDefinitionExpression definitionExpression, IReadOnlyList<Expression> args) : base(args)
    {
        this.definitionExpression = definitionExpression;
    }

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var expressionResult = await definitionExpression.EvaluateAsync(interpreter, cancellationToken);
        var definition = expressionResult.ExpectFunction();

        return await ExecuteFunction(definition, interpreter, cancellationToken);
    }

    protected override string DebugDisplay() => $"({definitionExpression})({string.Join(',', Args)})";

    protected override TokenLocation? GetCallLocation() => definitionExpression.Location;
}