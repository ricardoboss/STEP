using StepLang.Interpreting;
using StepLang.Parsing.Statements;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Expressions;

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

        interpreter.PushScope();

        await EvaluateParameters(interpreter, arguments, cancellationToken);

        await interpreter.InterpretAsync(body.ToAsyncEnumerable(), cancellationToken);

        return interpreter.PopScope().TryGetResult(out var result) ? result : ExpressionResult.Void;
    }

    private async Task EvaluateParameters(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        for (var i = 0; i < parameters.Count; i++)
        {
            var (parameterTypeToken, parameterNameToken) = parameters[i];
            var parameterType = parameterTypeToken.Value;
            var parameterName = parameterNameToken.Value;

            var argument = await arguments[i].EvaluateAsync(interpreter, cancellationToken);

            argument.ThrowIfVoid();

            if (argument.ValueType != parameterType)
                throw new InvalidArgumentTypeException(parameterTypeToken, argument);

            ExpressionResult parameterValue = ExpressionResult.From(parameterType, argument.Value);

            interpreter.CurrentScope.SetVariable(parameterName, parameterValue);
        }
    }

    protected override string DebugParamsString => string.Join(", ", parameters.Select(t => $"{t.type} {t.identifier}"));

    protected override string DebugBodyString => $"[{body.Count} statements]";
}