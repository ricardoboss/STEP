using StepLang.Tooling.Formatting.Fixers;

namespace StepLang.Tooling.Formatting.Tests.Fixers;

public class TypeNameCasingFixerTest
{
    [Theory]
    [InlineData("a", "a")]
    [InlineData(" String", " string")]
    [InlineData("NUMBER", "number")]
    [InlineData("bOOl ", "bool ")]
    [InlineData("\"Don't fix within String\"", "\"Don't fix within String\"")]
    [InlineData("\"Nested \\\"Bool\\\" String\"", "\"Nested \\\"Bool\\\" String\"")]
    public async Task TestKeywordCasingFixer(string input, string output)
    {
        var fixer = new TypeNameCasingFixer();

        var result = await fixer.FixAsync(input, CancellationToken.None);

        Assert.Equal(output, result.FixedString);
        Assert.Equal(!string.Equals(input, output, StringComparison.Ordinal), result.FixRequired);
    }
}