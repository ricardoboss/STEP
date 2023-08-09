using System.Diagnostics;
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

        return interpreter.PopScope().TryGetResult(out var result) ? result : VoidResult.Instance;
    }

    private async Task EvaluateParameters(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        for (var i = 0; i < parameters.Count; i++)
        {
            var (parameterTypeToken, parameterNameToken) = parameters[i];
            var argumentExpression = arguments[i];

            await EvaluateParameter(interpreter, parameterTypeToken, parameterNameToken, argumentExpression, cancellationToken);
        }
    }

    private static async Task EvaluateParameter(Interpreter interpreter, Token typeToken, Token nameToken, Expression argument, CancellationToken cancellationToken = default)
    {
        Debug.Assert(typeToken.Type == TokenType.TypeName, "typeToken.Type == TokenType.TypeName");

        var parameterType = ValueTypeExtensions.FromTypeName(typeToken.Value);
        var parameterName = nameToken.Value;

        var argumentResult = await argument.EvaluateAsync(interpreter, cancellationToken);
        if (argumentResult is VoidResult || argumentResult.ResultType != parameterType)
            throw new InvalidArgumentTypeException(typeToken, argumentResult);

        interpreter.CurrentScope.SetVariable(parameterName, argumentResult);
    }

    protected override string DebugParamsString => string.Join(", ", parameters.Select(t => $"{t.type} {t.identifier}"));

    protected override string DebugBodyString => $"[{body.Count} statements]";
}