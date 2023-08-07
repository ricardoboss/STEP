using System.Diagnostics.CodeAnalysis;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

public class FunctionCallStatement : Statement
{
    private readonly Token identifier;
    private readonly IReadOnlyList<Expression> args;

    /// <inheritdoc />
    public FunctionCallStatement(Token identifier, IReadOnlyList<Expression> args) : base(StatementType.FunctionCall)
    {
        if (identifier.Type != TokenType.Identifier)
            throw new UnexpectedTokenException(identifier, TokenType.Identifier);

        this.identifier = identifier;
        this.args = args;
    }

    /// <inheritdoc />
    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var functionExpression = new IdentifierFunctionCallExpression(identifier, args);

        var result = await functionExpression.EvaluateAsync(interpreter, cancellationToken);

        result.ThrowIfNotVoid();
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugRenderContent()
    {
        return $"{identifier}({string.Join(',', args)})";
    }
}