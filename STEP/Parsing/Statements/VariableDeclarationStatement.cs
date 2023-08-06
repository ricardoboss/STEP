using System.Diagnostics.CodeAnalysis;
using STEP.Interpreting;
using STEP.Parsing.Expressions;
using STEP.Tokenizing;

namespace STEP.Parsing.Statements;

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

            interpreter.CurrentScope.SetVariable(identifier.Value, ExpressionResult.From(type.Value));

            return;
        }

        var result = await expression.EvaluateAsync(interpreter, cancellationToken);

        if (result.ValueType != type.Value)
            throw new IncompatibleTypesException(result.ValueType, type.Value, "assign");

        interpreter.CurrentScope.SetVariable(identifier.Value, result);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    protected override string DebugRenderContent()
    {
        var expressionStr = string.Empty;
        if (expression is not null)
            expressionStr = $" = {expression}";

        return $"{type} {identifier}{expressionStr}";
    }
}