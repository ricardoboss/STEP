using System.Diagnostics.CodeAnalysis;
using StepLang.Interpreting;
using StepLang.Parsing.Statements;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Expressions;

public class FunctionDefinitionExpression : Expression
{
    private readonly IReadOnlyList<(Token type, Token identifier)> parameters;
    private readonly IReadOnlyList<Statement> body;

    public TokenLocation? Location { get; init; }

    public FunctionDefinitionExpression(IReadOnlyList<(Token type, Token identifier)> parameters, IReadOnlyList<Statement> body)
    {
        this.parameters = parameters;
        this.body = body;
    }

    /// <inheritdoc />
    public override Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var function = new UserDefinedFunctionDefinition(parameters, body);
        var result = new FunctionResult(function);

        return Task.FromResult<ExpressionResult>(result);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugDisplay()
    {
        var paramsString = string.Join(", ", parameters.Select(t => $"{t.type} {t.identifier}"));
        var bodyString = $"[{body.Count} statements]";

        return $"({paramsString}) {{ {bodyString} }}";
    }
}