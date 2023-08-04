using HILFE.Interpreting;
using HILFE.Parsing.Statements;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Expressions;

public class UserDefinedFunctionDefinition : FunctionDefinition
{
    private readonly IReadOnlyList<(Token type, Token identifier)> parameters;
    private readonly IReadOnlyList<Statement> body;

    public UserDefinedFunctionDefinition(IReadOnlyList<(Token type, Token identifier)> parameters, IReadOnlyList<Statement> body)
    {
        this.parameters = parameters;
        this.body = body;
    }

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        if (arguments.Count != parameters.Count)
            throw new InvalidArgumentCountException(parameters.Count, arguments.Count);

        for (var i = 0; i < parameters.Count; i++)
        {
            var (parameterType, parameterName) = parameters[i];
            var argument = await arguments[i].EvaluateAsync(interpreter, cancellationToken);

            argument.ThrowIfVoid();

            if (argument.ValueType != parameterType.Value)
                throw new IncompatibleTypesException(argument.ValueType, parameterType.Value, "assign");

            interpreter.CurrentScope.SetVariable(parameterName.Value, ExpressionResult.From(parameterType.Value, argument.Value));
        }

        interpreter.PushScope();

        await interpreter.InterpretAsync(body.ToAsyncEnumerable(), cancellationToken);

        return interpreter.PopScope().TryGetResult(out var result) ? result : ExpressionResult.Void;
    }

    protected override string DebugParamsString => string.Join(", ", parameters.Select(t => $"{t.type} {t.identifier}"));

    protected override string DebugBodyString => $"[{body.Count} statements]";
}