using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Statements;

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
            interpreter.CurrentScope.UpdateValue(identifierToken, result, onlyCurrentScope: false);
        }
        catch (IncompatibleVariableTypeException e)
        {
            throw new InvalidVariableAssignmentException(identifierToken, e);
        }
        catch (NonNullableVariableAssignmentException e)
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