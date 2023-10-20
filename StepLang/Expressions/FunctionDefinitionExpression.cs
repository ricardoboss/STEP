using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Statements;
using StepLang.Tokenizing;

namespace StepLang.Expressions;

public class FunctionDefinitionExpression : Expression
{
    private readonly IReadOnlyList<VariableDeclarationExpression> parameters;
    private readonly IReadOnlyList<Statement> body;

    public TokenLocation? Location { get; init; }

    public FunctionDefinitionExpression(IReadOnlyList<VariableDeclarationExpression> parameters, IReadOnlyList<Statement> body)
    {
        this.parameters = parameters;
        this.body = body;
    }

    /// <inheritdoc />
    public override Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var result = new UserDefinedFunctionDefinition(parameters, body).ToResult();

        return Task.FromResult<ExpressionResult>(result);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugDisplay()
    {
        var paramsString = string.Join(", ", parameters.Select(t => t.ToString()));
        var bodyString = $"[{body.Count} statements]";

        return $"({paramsString}) {{ {bodyString} }}";
    }
}