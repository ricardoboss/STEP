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