using StepLang.Tooling.Formatting.Fixers;

namespace StepLang.Tooling.Formatting.Tests.Fixers;

public class LineEndingFixerTest
{
    [Theory]
    [InlineData("\r\n", "\n")]
    [InlineData("\n", "\n")]
    [InlineData("\r\n\r\n", "\n\n")]
    public async Task TestLineEndingFixerFixesLineEndings(string input, string output)
    {
        var fixer = new LineEndingFixer();

        var result = await fixer.FixAsync(input, CancellationToken.None);

        Assert.Equal(output, result.FixedString);
        Assert.Equal(!string.Equals(input, output, StringComparison.Ordinal), result.FixRequired);
    }
}