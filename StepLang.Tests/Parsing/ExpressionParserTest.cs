using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Tests.Parsing;

public class ExpressionParserTest
{
    [Fact]
    public async Task TestParseVariableExpression()
    {
        var interpreter = new Interpreter();
        interpreter.CurrentScope.CreateVariable(new(TokenType.Identifier, "variable"), ResultType.Bool, BoolResult.True);
        var expression = await ExpressionParser.ParseAsync(new[] { new Token(TokenType.Identifier, "variable") });

        var result = await expression.EvaluateAsync(interpreter);

        Assert.True(result is BoolResult { Value: true });
    }

    [Fact]
    public async Task TestParseSimpleAddition()
    {
        var interpreter = new Interpreter();
        var expression = await ExpressionParser.ParseAsync(new[] { new Token(TokenType.LiteralNumber, "123"), new Token(TokenType.PlusSymbol, "+"), new Token(TokenType.LiteralNumber, "456") });

        var result = await expression.EvaluateAsync(interpreter);

        Assert.True(result is NumberResult { Value: 579 });
    }

    [Fact]
    public async Task TestParseAdditionWithMultipleSummands()
    {
        var interpreter = new Interpreter();
        var expression = await ExpressionParser.ParseAsync(new[] { new Token(TokenType.LiteralNumber, "1"), new Token(TokenType.PlusSymbol, "+"), new Token(TokenType.LiteralNumber, "2"), new Token(TokenType.PlusSymbol, "+"), new Token(TokenType.LiteralNumber, "3"), new Token(TokenType.PlusSymbol, "+"), new Token(TokenType.LiteralNumber, "4") });

        var result = await expression.EvaluateAsync(interpreter);

        Assert.True(result is NumberResult { Value: 10 });
    }

    [Fact]
    public async Task TestMultiplicativeAdditivePrecedences()
    {
        var interpreter = new Interpreter();
        var expression = await ExpressionParser.ParseAsync(new[] { new Token(TokenType.LiteralNumber, "1"), new Token(TokenType.AsteriskSymbol, "*"), new Token(TokenType.LiteralNumber, "2"), new Token(TokenType.PlusSymbol, "+"), new Token(TokenType.LiteralNumber, "3") });

        var result = await expression.EvaluateAsync(interpreter);

        Assert.True(result is NumberResult { Value: 5 });
    }

    [Fact]
    public async Task TestAdditiveMultiplicativePrecedences()
    {
        var interpreter = new Interpreter();
        var expression = await ExpressionParser.ParseAsync(new[] { new Token(TokenType.LiteralNumber, "1"), new Token(TokenType.PlusSymbol, "+"), new Token(TokenType.LiteralNumber, "2"), new Token(TokenType.AsteriskSymbol, "*"), new Token(TokenType.LiteralNumber, "3") });

        var result = await expression.EvaluateAsync(interpreter);

        Assert.True(result is NumberResult { Value: 7 });
    }

    [Fact]
    public async Task TestAdditiveMultiplicativePrecedencesWithParentheses()
    {
        var interpreter = new Interpreter();
        var expression = await ExpressionParser.ParseAsync(new[] { new Token(TokenType.OpeningParentheses, "("), new Token(TokenType.LiteralNumber, "1"), new Token(TokenType.PlusSymbol, "+"), new Token(TokenType.LiteralNumber, "2"), new Token(TokenType.ClosingParentheses, ")"), new Token(TokenType.AsteriskSymbol, "*"), new Token(TokenType.LiteralNumber, "3") });

        var result = await expression.EvaluateAsync(interpreter);

        Assert.True(result is NumberResult { Value: 9 });
    }

    [Fact]
    public async Task TestParseThrowsUnexpectedEndOfTokensExceptionForEmptyExpression()
    {
        var exception = await Assert.ThrowsAsync<UnexpectedEndOfTokensException>(() => ExpressionParser.ParseAsync(Array.Empty<Token>()));

        Assert.Equal("PAR002", exception.ErrorCode);
    }

    [Fact]
    public async Task TestParseThrowsUnexpectedEndOfTokensExceptionForMissingRightOperand()
    {
        var exception = await Assert.ThrowsAsync<UnexpectedEndOfTokensException>(() => ExpressionParser.ParseAsync(new[] { new Token(TokenType.LiteralNumber, "1"), new Token(TokenType.PlusSymbol, "+") }));

        Assert.Equal("PAR002", exception.ErrorCode);
    }

    [Fact]
    public async Task TestInvalidExpressionIsThrownForEmptyParentheses()
    {
        var exception = await Assert.ThrowsAsync<InvalidExpressionException>(() => ExpressionParser.ParseAsync(new[] { new Token(TokenType.OpeningParentheses, "("), new Token(TokenType.ClosingParentheses, ")") }));

        Assert.Equal("PAR008", exception.ErrorCode);
    }
}