using StepLang.Tooling.Formatting.Fixers;

namespace StepLang.Tooling.Formatting.Tests.Fixers;

public class TrailingWhitespaceFixerTest
{
    [Theory]
    [InlineData("a", "a")]
    [InlineData("a ", "a")]
    [InlineData(" a", " a")]
    [InlineData(" a ", " a")]
    [InlineData("a\n", "a\n")]
    [InlineData("a\r\n", "a\r\n")]
    [InlineData("a\r\n ", "a\r\n")]
    [InlineData("a\r\n  ", "a\r\n")]
    [InlineData("a\n b\n ", "a\n b\n")]
    [InlineData("a \n b\n", "a\n b\n")]
    public async Task TestApplyRemovesTrailingWhitespace(string input, string output)
    {
        var fixer = new TrailingWhitespaceFixer();

        var result = await fixer.FixAsync(input, CancellationToken.None);

        Assert.Equal(!string.Equals(input, output, StringComparison.Ordinal), result.FixRequired);
        Assert.Equal(output, result.FixedString);
    }
}