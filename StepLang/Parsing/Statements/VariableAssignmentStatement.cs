using System.Diagnostics.CodeAnalysis;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

public class VariableAssignmentStatement : Statement
{
    private readonly Token identifierToken;
    private readonly Expression expression;

    /// <inheritdoc />
    public VariableAssignmentStatement(Token identifierToken, Expression expression) : base(StatementType.VariableAssignment)
    {
        if (identifierToken.Type != TokenType.Identifier)
            throw new UnexpectedTokenException(identifierToken, TokenType.Identifier);

        this.identifierToken = identifierToken;
        this.expression = expression;

        Location = identifierToken.Location;
    }

    /// <inheritdoc />
    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        var result = await expression.EvaluateAsync(interpreter, cancellationToken);

        try
        {
            interpreter.CurrentScope.UpdateValue(identifierToken, result);
        }
        catch (IncompatibleVariableTypeException e)
        {
            throw new InvalidVariableAssignmentException(identifierToken, e);
        }
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugRenderContent()
    {
        return $"{identifierToken} = {expression}";
    }
}