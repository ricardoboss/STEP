using HILFE.Interpreting;
using HILFE.Parsing.Statements;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Expressions;

public class FunctionDefinitionExpression : Expression
{
    private readonly IReadOnlyList<(Token type, Token identifier)> parameters;
    private readonly IReadOnlyList<Statement> body;

    public FunctionDefinitionExpression(IReadOnlyList<(Token type, Token identifier)> parameters, IReadOnlyList<Statement> body)
    {
        this.parameters = parameters;
        this.body = body;
    }

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var function = new UserDefinedFunctionDefintion(parameters, body, interpreter.CurrentScope);

        return new("function", function);
    }

    /// <inheritdoc />
    protected override string DebugDisplay()
    {
        var paramsString = string.Join(", ", parameters.Select(t => $"{t.type} {t.identifier}"));
        var bodyString = $"[{body.Count} statements]";

        return $"({paramsString}) {{ {bodyString} }}";
    }
}