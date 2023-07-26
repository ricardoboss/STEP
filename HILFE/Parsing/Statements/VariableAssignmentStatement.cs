using System.Diagnostics.CodeAnalysis;
using HILFE.Interpreting;
using HILFE.Parsing.Expressions;
using HILFE.Tokenizing;

namespace HILFE.Parsing.Statements;

public class VariableAssignmentStatement : Statement
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
    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var result = await expression.EvaluateAsync(interpreter, default);
        if (result is { IsVoid: true })
            throw new InterpreterException("Cannot assign a void value to a variable");

        interpreter.CurrentScope.SetByIdentifier(identifier.Value, result.Value);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugRenderContent()
    {
        return $"{identifier} = {expression}";
    }
}