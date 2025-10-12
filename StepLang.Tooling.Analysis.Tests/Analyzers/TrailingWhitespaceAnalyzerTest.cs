using StepLang.Tooling.Analysis.Analyzers;

namespace StepLang.Tooling.Analysis.Tests.Analyzers;

public class TrailingWhitespaceAnalyzerTest
{
	[Theory]
	[InlineData("a", null)]
	[InlineData("a ", "a")]
	[InlineData(" a", null)]
	[InlineData(" a ", " a")]
	[InlineData("a\t", "a")]
	[InlineData("\ta", null)]
	[InlineData("\ta\t", "\ta")]
	[InlineData("a\n\t", "a\n")]
	[InlineData("a\n", null)]
	[InlineData("a\r\n", null)]
	[InlineData("a\r\n ", "a\r\n")]
	[InlineData("a\r\n  ", "a\r\n")]
	[InlineData("a\n b\n ", "a\n b\n")]
	[InlineData("a \n b\n", "a\n b\n")]
	[InlineData("a\t\n\tb\n", "a\n\tb\n")]
	public async Task TestTrailingWhitespaceAnalyzer(string input, string? output)
	{
		var fixer = new TrailingWhitespaceAnalyzer();

		var result = await fixer.AnalyzeAsync(input, CancellationToken.None);

		Assert.Equal(output, result.FixedString);
		Assert.Equal(output != null, result.ShouldFix);
	}
}
