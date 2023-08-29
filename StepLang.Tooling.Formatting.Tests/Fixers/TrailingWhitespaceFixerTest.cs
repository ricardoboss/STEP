using StepLang.Tooling.Formatting.Analyzers;

namespace StepLang.Tooling.Formatting.Tests.Fixers;

public class TrailingWhitespaceFixerTest
{
    [Theory]
    [InlineData("a", "a")]
    [InlineData("a ", "a")]
    [InlineData(" a", " a")]
    [InlineData(" a ", " a")]
    [InlineData("a\t", "a")]
    [InlineData("\ta", "\ta")]
    [InlineData("\ta\t", "\ta")]
    [InlineData("a\n\t", "a\n")]
    [InlineData("a\n", "a\n")]
    [InlineData("a\r\n", "a\r\n")]
    [InlineData("a\r\n ", "a\r\n")]
    [InlineData("a\r\n  ", "a\r\n")]
    [InlineData("a\n b\n ", "a\n b\n")]
    [InlineData("a \n b\n", "a\n b\n")]
    [InlineData("a\t\n\tb\n", "a\n\tb\n")]
    public async Task TestApplyRemovesTrailingWhitespace(string input, string output)
    {
        var fixer = new TrailingWhitespaceAnalyzer();

        var result = await fixer.AnalyzeAsync(input, CancellationToken.None);

        Assert.Equal(output, result.FixedString);
        Assert.Equal(!string.Equals(input, output, StringComparison.Ordinal), result.FixRequired);
    }
}