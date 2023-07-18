using HILFE.Interpreting;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public class VariableAssignmentStatement : Statement, IExecutableStatement
{
    private readonly Token identifier;
    private readonly Expression valueExpression;

    /// <inheritdoc />
    public VariableAssignmentStatement(Token identifier, IReadOnlyList<Token> valueExpression) : base(StatementType.VariableDeclaration)
    {
        if (identifier.Type != TokenType.Identifier)
            throw new TokenizerException($"Expected {TokenType.Identifier} token, but got {identifier.Type} instead");

        this.identifier = identifier;

        this.valueExpression = new(valueExpression);
    }

    /// <inheritdoc />
    public Task ExecuteAsync(Interpreter interpreter)
    {
        var result = valueExpression.Evaluate(interpreter);
        if (result is { IsVoid: false })
            interpreter.CurrentScope.SetByIdentifier(identifier.Value, result.Value);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected override string DebugRenderContent()
    {
        return $"{identifier} = {valueExpression}";
    }
}