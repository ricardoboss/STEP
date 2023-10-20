using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions;
using StepLang.Interpreting;

namespace StepLang.Statements;

public class VariableDeclarationStatement : Statement
{
    private readonly VariableDeclarationExpression declarationExpression;
    private readonly Expression? expression;

    /// <inheritdoc />
    public VariableDeclarationStatement(VariableDeclarationExpression declarationExpression, Expression? expression) : base(StatementType.VariableDeclaration)
    {
        this.declarationExpression = declarationExpression;
        this.expression = expression;

        Location = declarationExpression.TypeToken.Location;
    }

    /// <inheritdoc />
    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        await declarationExpression.EvaluateAsync(interpreter, cancellationToken);

        // if there is no expression, we are done
        if (expression is null)
            return;

        var result = await expression.EvaluateAsync(interpreter, cancellationToken);

        try
        {
            // this will throw if the resulting expression type is not compatible with the variable type
            interpreter.CurrentScope.UpdateValue(declarationExpression.IdentifierToken, result, onlyCurrentScope: false);
        }
        catch (IncompatibleVariableTypeException e)
        {
            throw new InvalidVariableAssignmentException(declarationExpression.TypeToken, e);
        }
        catch (NonNullableVariableAssignmentException e)
        {
            throw new InvalidVariableAssignmentException(declarationExpression.TypeToken, e);
        }
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugRenderContent()
    {
        var expressionStr = string.Empty;
        if (expression is not null)
            expressionStr = $" = {expression}";

        return $"{declarationExpression}{expressionStr}";
    }
}