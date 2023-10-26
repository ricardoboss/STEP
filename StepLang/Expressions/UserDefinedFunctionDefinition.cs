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

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters =>
        parameters.Select(t =>
        {
            var allowedTypes = new List<ResultType>(2)
            {
                ValueTypeExtensions.FromTypeName(t.TypeToken.Value),
            };

            if (t.NullabilityIndicatorToken is not null)
                allowedTypes.Add(ResultType.Null);

            return (allowedTypes.ToArray(), t.IdentifierToken.Value);
        });

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        if (arguments.Count != parameters.Count)
            throw new InvalidArgumentCountException(parameters.Count, arguments.Count);

        // evaluate args before pushing scope
        var evaldArgs = await arguments.EvaluateAsync(interpreter, cancellationToken).ToListAsync(cancellationToken);

        interpreter.PushScope();

        try
        {
            // create the parameter variables in the new scope
            await EvaluateParameters(interpreter, evaldArgs, cancellationToken);
        }
        catch (NonNullableVariableAssignmentException e)
        {
            throw new InvalidArgumentTypeException(null, new[] { e.Variable.Type }, e.NewValue);
        }

        await interpreter.InterpretAsync(body.ToAsyncEnumerable(), cancellationToken);

        return interpreter.PopScope().TryGetResult(out var result) ? result : VoidResult.Instance;
    }

    private async Task EvaluateParameters(Interpreter interpreter, IReadOnlyList<ExpressionResult> arguments, CancellationToken cancellationToken = default)
    {
        for (var i = 0; i < parameters.Count; i++)
        {
            var parameter = parameters[i];
            var argument = arguments[i];

            // this will create the variable in the current scope
            _ = await parameter.EvaluateAsync(interpreter, cancellationToken);

            // and set the value
            interpreter.CurrentScope.UpdateValue(parameter.IdentifierToken, argument);
        }
    }

    protected override string DebugParamsString => string.Join(", ", parameters.Select(t => t.ToString()));

    protected override string DebugBodyString => $"[{body.Count} statements]";
}