using HILFE.Interpreting;
using HILFE.Parsing.Expressions;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public class VariableDeclarationStatement : Statement
{
    private readonly Token type;
    private readonly Token identifier;
    private readonly Expression? expression;

    /// <inheritdoc />
    public VariableDeclarationStatement(Token type, Token identifier, Expression? expression) : base(StatementType.VariableDeclaration)
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
    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        if (expression is null)
        {
            // only declare, no value assigned

            interpreter.CurrentScope.SetVariable(new(identifier.Value, type.Value, null));

            return;
        }

        var result = await expression.EvaluateAsync(interpreter, cancellationToken);
        if (result is null or { IsVoid: true })
            throw new InterpreterException("Cannot assign a void value to a variable");

        interpreter.CurrentScope.SetVariable(new(identifier.Value, type.Value, result.Value));
    }

    /// <inheritdoc />
    protected override string DebugRenderContent()
    {
        var expressionStr = string.Empty;
        if (expression is not null)
            expressionStr = $" = {expression}";

        return $"{type} {identifier}{expressionStr}";
    }
}