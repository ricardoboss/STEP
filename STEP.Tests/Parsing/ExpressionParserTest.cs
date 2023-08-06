using STEP.Interpreting;
using STEP.Parsing.Expressions;
using STEP.Tokenizing;

namespace STEP.Tests.Parsing;

public class ExpressionParserTest
{
    [Fact]
    public async Task TestParseVariableExpression()
    {
        var interpreter = new Interpreter();
        interpreter.CurrentScope.SetVariable("variable", ExpressionResult.True);
        var expression = await ExpressionParser.ParseAsync(new [] { new Token(TokenType.Identifier, "variable") });


        var result = await expression.EvaluateAsync(interpreter);

        Assert.False(result.IsVoid);
        Assert.Equal(true, result.Value);
        Assert.Equal("bool", result.ValueType);
    }

    [Fact]
    public async Task TestParseSimpleAddition()
    {
        var interpreter = new Interpreter();
        var expression = await ExpressionParser.ParseAsync(new [] { new Token(TokenType.LiteralNumber, "123"), new Token(TokenType.PlusSymbol, "+"), new Token(TokenType.LiteralNumber, "456") });

        var result = await expression.EvaluateAsync(interpreter);

        Assert.False(result.IsVoid);
        Assert.Equal(579, result.Value);
        Assert.Equal("number", result.ValueType);
    }

    [Fact]
    public async Task TestParseAdditionWithMultipleSummands()
    {
        var interpreter = new Interpreter();
        var expression = await ExpressionParser.ParseAsync(new [] { new Token(TokenType.LiteralNumber, "1"), new Token(TokenType.PlusSymbol, "+"), new Token(TokenType.LiteralNumber, "2"), new Token(TokenType.PlusSymbol, "+"), new Token(TokenType.LiteralNumber, "3"), new Token(TokenType.PlusSymbol, "+"), new Token(TokenType.LiteralNumber, "4") });

        var result = await expression.EvaluateAsync(interpreter);

        Assert.False(result.IsVoid);
        Assert.Equal(10, result.Value);
        Assert.Equal("number", result.ValueType);
    }

    [Fact]
    public async Task TestMultiplicativeAdditivePrecedences()
    {
        var interpreter = new Interpreter();
        var expression = await ExpressionParser.ParseAsync(new [] { new Token(TokenType.LiteralNumber, "1"), new Token(TokenType.AsteriskSymbol, "*"), new Token(TokenType.LiteralNumber, "2"), new Token(TokenType.PlusSymbol, "+"), new Token(TokenType.LiteralNumber, "3") });

        var result = await expression.EvaluateAsync(interpreter);

        Assert.False(result.IsVoid);
        Assert.Equal(5, result.Value);
        Assert.Equal("number", result.ValueType);
    }

    [Fact]
    public async Task TestAdditiveMultiplicativePrecedences()
    {
        var interpreter = new Interpreter();
        var expression = await ExpressionParser.ParseAsync(new [] { new Token(TokenType.LiteralNumber, "1"), new Token(TokenType.PlusSymbol, "+"), new Token(TokenType.LiteralNumber, "2"), new Token(TokenType.AsteriskSymbol, "*"), new Token(TokenType.LiteralNumber, "3") });

        var result = await expression.EvaluateAsync(interpreter);

        Assert.False(result.IsVoid);
        Assert.Equal(7, result.Value);
        Assert.Equal("number", result.ValueType);
    }

    [Fact]
    public async Task TestAdditiveMultiplicativePrecedencesWithParentheses()
    {
        var interpreter = new Interpreter();
        var expression = await ExpressionParser.ParseAsync(new [] { new Token(TokenType.OpeningParentheses, "("), new Token(TokenType.LiteralNumber, "1"), new Token(TokenType.PlusSymbol, "+"), new Token(TokenType.LiteralNumber, "2"), new Token(TokenType.ClosingParentheses, ")"), new Token(TokenType.AsteriskSymbol, "*"), new Token(TokenType.LiteralNumber, "3") });

        var result = await expression.EvaluateAsync(interpreter);

        Assert.False(result.IsVoid);
        Assert.Equal(9, result.Value);
        Assert.Equal("number", result.ValueType);
    }
}