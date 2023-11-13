using StepLang.Tokenizing;

namespace StepLang.Tooling.Highlighting.Tests;

public class HighlighterTest
{
    [Fact]
    public void TestHighlighterHighlightsSimpleTokens()
    {
        const string source = "number a = 1";

        var highlighter = new Highlighter(ColorScheme.Mono);
        var tokens = highlighter.Highlight(source).ToList();

        Assert.Equal(8, tokens.Count);
    }

    [Theory]
    [MemberData(nameof(ExplicitlyStyledTokenTypes))]
    public void TestStyleCoversEveryTokenType(TokenType type)
    {
        var scheme = ColorScheme.Mono;
        var style = Highlighter.GetStyle(type, scheme);

        Assert.False(style.IsDefault);
    }

    public static IEnumerable<object[]> ExplicitlyStyledTokenTypes()
    {
        return Enum.GetValues<TokenType>()
            .Except(new[]
            {
                TokenType.Whitespace,
                TokenType.NewLine,
                TokenType.EndOfFile,
            })
            .Select(t => new object[] { t });
    }
}
