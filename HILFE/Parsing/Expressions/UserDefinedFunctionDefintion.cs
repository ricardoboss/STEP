using HILFE.Interpreting;
using HILFE.Parsing.Statements;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Expressions;

public class UserDefinedFunctionDefintion : FunctionDefinition
{
    private readonly IReadOnlyList<(Token type, Token identifier)> parameters;
    private readonly IReadOnlyList<Statement> body;
    private readonly Scope parentScope;

    public UserDefinedFunctionDefintion(IReadOnlyList<(Token type, Token identifier)> parameters, IReadOnlyList<Statement> body, Scope parentScope)
    {
        this.parameters = parameters;
        this.body = body;
        this.parentScope = parentScope;
    }

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        if (arguments.Count != parameters.Count)
            throw new InterpreterException($"Expected {parameters.Count} arguments, but got {arguments.Count} instead");

        for (var i = 0; i < parameters.Count; i++)
        {
            var (parameterType, parameterName) = parameters[i];
            var argument = await arguments[i].EvaluateAsync(interpreter, cancellationToken);

            if (argument.IsVoid)
                throw new InterpreterException("Cannot assign a void value to a variable");

            if (argument.ValueType != parameterType.Value)
                throw new InterpreterException($"Expected argument of type {parameterType.Value}, but got {argument.ValueType} instead");

            interpreter.CurrentScope.SetVariable(new(parameterName.Value, parameterType.Value, argument.Value));
        }

        interpreter.PushScope();

        await interpreter.InterpretAsync(body.ToAsyncEnumerable(), cancellationToken);

        return interpreter.PopScope().TryGetResult(out var result) ? result : ExpressionResult.Void;
    }

    protected override string DebugParamsString => string.Join(", ", parameters.Select(t => $"{t.type} {t.identifier}"));

    protected override string DebugBodyString => $"[{body.Count} statements]";
}