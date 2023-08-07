using System.Diagnostics.CodeAnalysis;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

public class VariableAssignmentStatement : Statement
{
    private readonly Token identifier;
    private readonly Expression expression;

    /// <inheritdoc />
    public VariableAssignmentStatement(Token identifier, Expression expression) : base(StatementType.VariableAssignment)
    {
        if (identifier.Type != TokenType.Identifier)
            throw new UnexpectedTokenException(identifier, TokenType.Identifier);

        this.identifier = identifier;
        this.expression = expression;

        Location = identifier.Location;
    }

    /// <inheritdoc />
    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var result = await expression.EvaluateAsync(interpreter, cancellationToken);

        interpreter.CurrentScope.UpdateValue(identifier, result);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugRenderContent()
    {
        return $"{identifier} = {expression}";
    }
}