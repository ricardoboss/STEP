using System.Diagnostics.CodeAnalysis;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

public class VariableDeclarationStatement : Statement
{
    private readonly Token typeToken;
    private readonly Token identifierToken;
    private readonly Expression? expression;

    /// <inheritdoc />
    public VariableDeclarationStatement(Token typeToken, Token identifierToken, Expression? expression) : base(StatementType.VariableDeclaration)
    {
        if (typeToken.Type != TokenType.TypeName)
            throw new UnexpectedTokenException(typeToken, TokenType.TypeName);

        this.typeToken = typeToken;

        if (identifierToken.Type != TokenType.Identifier)
            throw new UnexpectedTokenException(identifierToken, TokenType.Identifier);

        this.identifierToken = identifierToken;
        this.expression = expression;

        Location = typeToken.Location;
    }

    /// <inheritdoc />
    public override async Task ExecuteAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        // default value for given type
        var result = ExpressionResult.DefaultFor(ValueTypeExtensions.FromTypeName(typeToken.Value));

        // add variable to current scope
        interpreter.CurrentScope.SetVariable(identifierToken.Value, result);

        // if there is no expression, we are done
        if (expression is null)
            return;

        result = await expression.EvaluateAsync(interpreter, cancellationToken);

        try
        {
            // this will throw if the resulting expression type is not compatible with the variable type
            interpreter.CurrentScope.UpdateValue(identifierToken, result);
        }
        catch (IncompatibleVariableTypeException e)
        {
            throw new InvalidVariableAssignmentException(typeToken, e);
        }
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugRenderContent()
    {
        var expressionStr = string.Empty;
        if (expression is not null)
            expressionStr = $" = {expression}";

        return $"{typeToken} {identifierToken}{expressionStr}";
    }
}