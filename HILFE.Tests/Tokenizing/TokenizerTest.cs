using HILFE.Tokenizing;

namespace HILFE.Tests.Tokenizing;

public class TokenizerTest
{
    [Fact]
    public async Task TestTokenizeLiteralString()
    {
        var source = "\"abc\"".ToAsyncEnumerable();
        var tokenizer = new Tokenizer();
        var tokens = await tokenizer.TokenizeAsync(source, CancellationToken.None).ToArrayAsync();

        Assert.Single(tokens);
        Assert.Equal(TokenType.LiteralString, tokens[0].Type);
        Assert.Equal("abc", tokens[0].Value);
    }

    [Fact]
    public async Task TestTokenizeLiteralStringWithSpaces()
    {
        var source = "\"abc def\"".ToAsyncEnumerable();
        var tokenizer = new Tokenizer();
        var tokens = await tokenizer.TokenizeAsync(source, CancellationToken.None).ToArrayAsync();

        Assert.Single(tokens);
        Assert.Equal(TokenType.LiteralString, tokens[0].Type);
        Assert.Equal("abc def", tokens[0].Value);
    }

    [Theory]
    [InlineData("\"abc\\\"def\"", "abc\"def")]
    [InlineData("\'abc\\\'def'", "abc'def")]
    public async Task TestTokenizeLiteralStringWithEscapedQuotes(string source, string expected)
    {
        var tokenizer = new Tokenizer();

        var tokens = await tokenizer.TokenizeAsync(source.ToAsyncEnumerable(), CancellationToken.None).ToArrayAsync();

        Assert.Single(tokens);
        Assert.Equal(TokenType.LiteralString, tokens[0].Type);
        Assert.Equal(expected, tokens[0].Value);
    }

    [Theory]
    [InlineData("string")]
    [InlineData("double")]
    [InlineData("int")]
    [InlineData("bool")]
    public async Task TestTokenizeKnownType(string source)
    {
        var tokenizer = new Tokenizer();
        var tokens = await tokenizer.TokenizeAsync(source.ToAsyncEnumerable(), CancellationToken.None).ToListAsync();

        Assert.Single(tokens);
        Assert.Equal(TokenType.TypeName, tokens[0].Type);
        Assert.Equal(source, tokens[0].Value);
    }

    [Theory]
    [InlineData("if", TokenType.IfKeyword)]
    [InlineData("else", TokenType.ElseKeyword)]
    [InlineData("while", TokenType.WhileKeyword)]
    [InlineData("break", TokenType.BreakKeyword)]
    [InlineData("continue", TokenType.ContinueKeyword)]
    public async Task TestTokenizeKeyword(string source, TokenType type)
    {
        var tokenizer = new Tokenizer();
        var tokens = await tokenizer.TokenizeAsync(source.ToAsyncEnumerable(), CancellationToken.None).ToArrayAsync();

        Assert.Single(tokens);
        Assert.Equal(type, tokens[0].Type);
        Assert.Equal(source, tokens[0].Value);
    }

    [Fact]
    public async Task TestTokenizeMultipleTokens()
    {
        const string source = "double identifier = 1";

        var tokenizer = new Tokenizer();
        var tokens = await tokenizer.TokenizeAsync(source.ToAsyncEnumerable(), CancellationToken.None).ToListAsync();

        Assert.Equal(7, tokens.Count);
        Assert.Equal(TokenType.TypeName, tokens[0].Type);
        Assert.Equal("double", tokens[0].Value);
        Assert.Equal(TokenType.Whitespace, tokens[1].Type);
        Assert.Equal(" ", tokens[1].Value);
        Assert.Equal(TokenType.Identifier, tokens[2].Type);
        Assert.Equal("identifier", tokens[2].Value);
        Assert.Equal(TokenType.Whitespace, tokens[3].Type);
        Assert.Equal(" ", tokens[3].Value);
        Assert.Equal(TokenType.EqualsSymbol, tokens[4].Type);
        Assert.Equal("=", tokens[4].Value);
        Assert.Equal(TokenType.Whitespace, tokens[5].Type);
        Assert.Equal(" ", tokens[5].Value);
        Assert.Equal(TokenType.LiteralNumber, tokens[6].Type);
        Assert.Equal("1", tokens[6].Value);
    }

    [Fact]
    public async Task TestTokenizeKeywords()
    {
        const string source = "if (true)";

        var tokenizer = new Tokenizer();
        var tokens = await tokenizer.TokenizeAsync(source.ToAsyncEnumerable(), CancellationToken.None).ToListAsync();

        Assert.Equal(5, tokens.Count);
        Assert.Equal(TokenType.IfKeyword, tokens[0].Type);
        Assert.Equal("if", tokens[0].Value);
        Assert.Equal(TokenType.Whitespace, tokens[1].Type);
        Assert.Equal(" ", tokens[1].Value);
        Assert.Equal(TokenType.ExpressionOpener, tokens[2].Type);
        Assert.Equal("(", tokens[2].Value);
        Assert.Equal(TokenType.LiteralBoolean, tokens[3].Type);
        Assert.Equal("true", tokens[3].Value);
        Assert.Equal(TokenType.ExpressionCloser, tokens[4].Type);
        Assert.Equal(")", tokens[4].Value);
    }

    [Fact]
    public async Task TestTokenizeFunctionCall()
    {
        const string source = "print(\"hello\")";

        var tokenizer = new Tokenizer();
        var tokens = await tokenizer.TokenizeAsync(source.ToAsyncEnumerable(), CancellationToken.None).ToListAsync();

        Assert.Equal(4, tokens.Count);
        Assert.Equal(TokenType.Identifier, tokens[0].Type);
        Assert.Equal("print", tokens[0].Value);
        Assert.Equal(TokenType.ExpressionOpener, tokens[1].Type);
        Assert.Equal("(", tokens[1].Value);
        Assert.Equal(TokenType.LiteralString, tokens[2].Type);
        Assert.Equal("hello", tokens[2].Value);
        Assert.Equal(TokenType.ExpressionCloser, tokens[3].Type);
        Assert.Equal(")", tokens[3].Value);
    }
}