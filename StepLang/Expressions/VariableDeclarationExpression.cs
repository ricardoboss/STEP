using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Statements;
using StepLang.Tokenizing;

namespace StepLang.Expressions;

public class VariableDeclarationExpression : Expression
{
    public Token TypeToken { get; }
    public Token IdentifierToken { get; }
    public Token? NullableIndicatorToken { get; }

    public bool Nullable => NullableIndicatorToken is not null;

    public VariableDeclarationExpression(Token typeToken, Token identifierToken, Token? nullableIndicatorToken)
    {
        if (typeToken.Type != TokenType.TypeName)
            throw new UnexpectedTokenException(typeToken, TokenType.TypeName);

        TypeToken = typeToken;

        if (identifierToken.Type != TokenType.Identifier)
            throw new UnexpectedTokenException(identifierToken, TokenType.Identifier);

        IdentifierToken = identifierToken;

        if (nullableIndicatorToken == null)
            return;

        if (nullableIndicatorToken.Type != TokenType.QuestionMarkSymbol)
            throw new UnexpectedTokenException(nullableIndicatorToken, TokenType.QuestionMarkSymbol);

        NullableIndicatorToken = nullableIndicatorToken;
    }

    /// <inheritdoc />
    public override Task<ExpressionResult> EvaluateAsync(Interpreter interpreter,
        CancellationToken cancellationToken = default)
    {
        if (interpreter.CurrentScope.Exists(IdentifierToken.Value, false))
            throw new VariableAlreadyDeclaredException(IdentifierToken);

        var type = ValueTypeExtensions.FromTypeName(TypeToken.Value);

        // default value for given type
        var value = Nullable ?
            NullResult.Instance :
            ExpressionResult.DefaultFor(type);

        // add variable to current scope
        interpreter.CurrentScope.CreateVariable(IdentifierToken, type, value, Nullable);

        return Task.FromResult(value);
    }

    /// <inheritdoc />
    protected override string DebugDisplay() => $"{TypeToken}{NullableIndicatorToken} {IdentifierToken}";
}