using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public class VariableDeclarationStatement : Statement, IExecutableStatement
{
    private readonly Token type;
    private readonly Token identifier;
    private readonly Expression expression;

    /// <inheritdoc />
    public VariableDeclarationStatement(Token type, Token identifier, Expression expression) : base(StatementType.VariableDeclaration)
    {
        if (type.Type != TokenType.TypeName)
            throw new UnexpectedTokenException(type, TokenType.TypeName);

        this.type = type;

        if (identifier.Type != TokenType.Identifier)
            throw new UnexpectedTokenException(identifier, TokenType.Identifier);

        this.identifier = identifier;
        this.expression = expression;
    }

    /// <inheritdoc />
    public Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var result = expression.Evaluate(interpreter);
        if (result is { IsVoid: false })
            interpreter.CurrentScope.AddIdentifier(identifier.Value, new(identifier.Value, type.Value, result.Value));

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected override string DebugRenderContent()
    {
        return $"{type} {identifier} = {expression}";
    }
}