using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public class FunctionCallStatement : Statement, IExecutableStatement
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
    public async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var functionExpression = new Expression.FunctionCallExpression(identifier, args);

        var result = await functionExpression.EvaluateAsync(interpreter, default);

        if (!result.IsVoid)
            throw new InterpreterException($"function call returned non-void value: {result}");
    }

    /// <inheritdoc />
    protected override string DebugRenderContent()
    {
        return $"{identifier}({string.Join(',', args)})";
    }
}