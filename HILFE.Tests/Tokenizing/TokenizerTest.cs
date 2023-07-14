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
        var doubleQuoteString = "\"abc\\\"def\"".ToAsyncEnumerable();
        var doubleQuoteTokens = await Tokenizer.TokenizeAsync(doubleQuoteString, CancellationToken.None).ToArrayAsync();

        Assert.Single(doubleQuoteTokens);
        Assert.Equal(TokenType.LiteralString, doubleQuoteTokens[0].Type);
        Assert.Equal("abc\"def", doubleQuoteTokens[0].Value);

        var singleQuoteString = "\'abc\\\'def'".ToAsyncEnumerable();
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
            var sourceChars = source.ToAsyncEnumerable();
            var tokens = await Tokenizer.TokenizeAsync(sourceChars, CancellationToken.None).ToArrayAsync();

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
            var sourceChars = source.Key.ToAsyncEnumerable();
            var tokens = await Tokenizer.TokenizeAsync(sourceChars, CancellationToken.None).ToArrayAsync();

            Assert.Single(tokens);
            Assert.Equal(source.Value, tokens[0].Type);
            Assert.Equal(source.Key, tokens[0].Value);
        }
    }
}