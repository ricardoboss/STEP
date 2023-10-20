using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Statements;

namespace StepLang.Expressions;

public class UserDefinedFunctionDefinition : FunctionDefinition
{
    private readonly IReadOnlyList<VariableDeclarationExpression> parameters;
    private readonly IReadOnlyList<Statement> body;

    public UserDefinedFunctionDefinition(IReadOnlyList<VariableDeclarationExpression> parameters, IReadOnlyList<Statement> body)
    {
        this.parameters = parameters;
        this.body = body;
    }

    // FIXME: this doesn't allow nullable types
    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => parameters.Select(t => (new[] { ValueTypeExtensions.FromTypeName(t.TypeToken.Value) }, t.IdentifierToken.Value));

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        if (arguments.Count != parameters.Count)
            throw new InvalidArgumentCountException(parameters.Count, arguments.Count);

        interpreter.PushScope();

        await EvaluateParameters(interpreter, arguments, cancellationToken);

        await interpreter.InterpretAsync(body.ToAsyncEnumerable(), cancellationToken);

        return interpreter.PopScope().TryGetResult(out var result) ? result : VoidResult.Instance;
    }

    private async Task EvaluateParameters(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        for (var i = 0; i < parameters.Count; i++)
        {
            var parameter = parameters[i];
            var argument = arguments[i];

            // this will create the variable in the current scope
            _ = await parameter.EvaluateAsync(interpreter, cancellationToken);

            var argumentValue = await argument.EvaluateAsync(interpreter, cancellationToken);

            interpreter.CurrentScope.UpdateValue(parameter.IdentifierToken, argumentValue);
        }
    }

    protected override string DebugParamsString => string.Join(", ", parameters.Select(t => t.ToString()));

    protected override string DebugBodyString => $"[{body.Count} statements]";
}