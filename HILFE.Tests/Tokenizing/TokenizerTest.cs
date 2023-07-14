using HILFE.Tokenizing;

namespace HILFE.Tests.Tokenizing;

public class TokenizerTest
{
    [Fact]
    public async Task TestTokenizeLiteralString()
    {
        var source = "\"abc\"".ToAsyncEnumerable();
        var tokens = await Tokenizer.TokenizeAsync(source, CancellationToken.None).ToArrayAsync();

        Assert.Single(tokens);
        Assert.Equal(TokenType.LiteralString, tokens[0].Type);
        Assert.Equal("abc", tokens[0].Value);
    }

    [Fact]
    public async Task TestTokenizeLiteralStringWithSpaces()
    {
        var source = "\"abc def\"".ToAsyncEnumerable();
        var tokens = await Tokenizer.TokenizeAsync(source, CancellationToken.None).ToArrayAsync();

        Assert.Single(tokens);
        Assert.Equal(TokenType.LiteralString, tokens[0].Type);
        Assert.Equal("abc def", tokens[0].Value);
    }

    [Fact]
    public async Task TestTokenizeLiteralStringWithEscapedQuotes()
    {
        const string doubleQuoteString = "\"abc\\\"def\"";
        var doubleQuoteTokens = await Tokenizer.TokenizeAsync(doubleQuoteString, CancellationToken.None).ToArrayAsync();

        Assert.Single(doubleQuoteTokens);
        Assert.Equal(TokenType.LiteralString, doubleQuoteTokens[0].Type);
        Assert.Equal("abc\"def", doubleQuoteTokens[0].Value);

        const string singleQuoteString = "\'abc\\\'def'";
        var singleQuoteTokens = await Tokenizer.TokenizeAsync(singleQuoteString, CancellationToken.None).ToArrayAsync();

        Assert.Single(singleQuoteTokens);
        Assert.Equal(TokenType.LiteralString, singleQuoteTokens[0].Type);
        Assert.Equal("abc'def", singleQuoteTokens[0].Value);
    }

    [Fact]
    public async Task TestTokenizeKnownType()
    {
        var sources = new [] { "string", "double", "int", "bool" };
        foreach (var source in sources)
        {
            var tokens = await Tokenizer.TokenizeAsync(source, CancellationToken.None).ToArrayAsync();

            Assert.Single(tokens);
            Assert.Equal(TokenType.TypeName, tokens[0].Type);
            Assert.Equal(source, tokens[0].Value);
        }
    }

    [Fact]
    public async Task TestTokenizeKeyword()
    {
        var sources = new Dictionary<string, TokenType>
        {
            { "if", TokenType.IfKeyword },
            { "else", TokenType.ElseKeyword },
            { "elseif", TokenType.ElseIfKeyword },
            { "while", TokenType.WhileKeyword },
            { "break", TokenType.BreakKeyword },
            { "continue", TokenType.ContinueKeyword },
        };

        foreach (var source in sources)
        {
            var tokens = await Tokenizer.TokenizeAsync(source.Key, CancellationToken.None).ToArrayAsync();

            Assert.Single(tokens);
            Assert.Equal(source.Value, tokens[0].Type);
            Assert.Equal(source.Key, tokens[0].Value);
        }
    }

    [Fact]
    public async Task TestTokenizeMultipleTokens()
    {
        const string source = "double identifier = 1";

        var tokens = await Tokenizer.TokenizeAsync(source, CancellationToken.None).ToArrayAsync();

        Assert.Equal(7, tokens.Length);
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

        var tokens = await Tokenizer.TokenizeAsync(source, CancellationToken.None).ToArrayAsync();

        Assert.Equal(5, tokens.Length);
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
}