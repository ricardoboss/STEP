using StepLang.Tooling.Analysis.Analyzers;

namespace StepLang.Tooling.Analysis.Tests.Analyzers;

public class TrailingWhitespaceAnalyzerTest
{
	[TestCase("a", null)]
	[TestCase("a ", "a")]
	[TestCase(" a", null)]
	[TestCase(" a ", " a")]
	[TestCase("a\t", "a")]
	[TestCase("\ta", null)]
	[TestCase("\ta\t", "\ta")]
	[TestCase("a\n\t", "a\n")]
	[TestCase("a\n", null)]
	[TestCase("a\r\n", null)]
	[TestCase("a\r\n ", "a\r\n")]
	[TestCase("a\r\n  ", "a\r\n")]
	[TestCase("a\n b\n ", "a\n b\n")]
	[TestCase("a \n b\n", "a\n b\n")]
	[TestCase("a\t\n\tb\n", "a\n\tb\n")]
	public async Task TestTrailingWhitespaceAnalyzer(string input, string? output)
	{
		var fixer = new TrailingWhitespaceAnalyzer();

		var result = await fixer.AnalyzeAsync(input, CancellationToken.None);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.FixedString, Is.EqualTo(output));
			Assert.That(result.ShouldFix, Is.EqualTo(output != null));
		}
	}
}
