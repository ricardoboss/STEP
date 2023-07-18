using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public class VariableAssignmentStatement : Statement, IExecutableStatement
{
    private readonly Token identifier;
    private readonly Expression expression;

    /// <inheritdoc />
    public VariableAssignmentStatement(Token identifier, Expression expression) : base(StatementType.VariableAssignment)
    {
        if (identifier.Type != TokenType.Identifier)
            throw new TokenizerException($"Expected {TokenType.Identifier} token, but got {identifier.Type} instead");

        this.identifier = identifier;
        this.expression = expression;
    }

    /// <inheritdoc />
    public Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var result = expression.Evaluate(interpreter);
        if (result is { IsVoid: false })
            interpreter.CurrentScope.SetByIdentifier(identifier.Value, result.Value);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected override string DebugRenderContent()
    {
        return $"{identifier} = {expression}";
    }
}