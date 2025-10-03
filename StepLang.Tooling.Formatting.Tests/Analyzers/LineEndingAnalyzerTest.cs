using StepLang.Tooling.Formatting.Analyzers;

namespace StepLang.Tooling.Formatting.Tests.Analyzers;

public class LineEndingAnalyzerTest
{
	[Theory]
	[InlineData("\r\n", "\n")]
	[InlineData("\n", "\n")]
	[InlineData("\r\n\r\n", "\n\n")]
	public async Task TestLineEndingAnalyzer(string input, string output)
	{
		var fixer = new LineEndingAnalyzer();

		var result = await fixer.AnalyzeAsync(input, CancellationToken.None);

		Assert.Equal(output, result.FixedString);
		Assert.Equal(!string.Equals(input, output, StringComparison.Ordinal), result.ShouldFix);
	}
}
