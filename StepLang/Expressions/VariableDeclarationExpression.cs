using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Statements;
using StepLang.Tokenizing;

namespace StepLang.Expressions;

public class VariableDeclarationExpression : Expression
{
    public Token Type { get; }
    public Token Identifier { get; }

    public VariableDeclarationExpression(Token type, Token identifier)
    {
        if (type.Type != TokenType.TypeName)
            throw new UnexpectedTokenException(type, TokenType.TypeName);

        Type = type;

        if (identifier.Type != TokenType.Identifier)
            throw new UnexpectedTokenException(identifier, TokenType.Identifier);

        Identifier = identifier;
    }

    /// <inheritdoc />
    public override Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, CancellationToken cancellationToken = default)
    {
        if (interpreter.CurrentScope.Exists(Identifier.Value, false))
        {
            throw new VariableAlreadyDeclaredException(Identifier);
        }

        // default value for given type
        var result = ExpressionResult.DefaultFor(ValueTypeExtensions.FromTypeName(Type.Value));

        // add variable to current scope
        interpreter.CurrentScope.SetVariable(Identifier.Value, result);

        return Task.FromResult(result);
    }

    /// <inheritdoc />
    protected override string DebugDisplay() => $"{Type} {Identifier}";
}