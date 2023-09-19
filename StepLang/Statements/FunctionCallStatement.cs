using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Statements;

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

        Location = identifier.Location;
    }

    /// <inheritdoc />
    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var functionExpression = new IdentifierFunctionCallExpression(identifier, args);

        var result = await functionExpression.EvaluateAsync(interpreter, cancellationToken);

        if (result is not VoidResult)
            throw new InvalidResultTypeException(result.ResultType, ResultType.Void);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugRenderContent()
    {
        return $"{identifier}({string.Join(',', args)})";
    }
}