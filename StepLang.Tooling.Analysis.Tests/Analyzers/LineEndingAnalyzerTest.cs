using StepLang.Tooling.Analysis.Analyzers;

namespace StepLang.Tooling.Analysis.Tests.Analyzers;

public class LineEndingAnalyzerTest
{
	[TestCase("\r\n", "\n")]
	[TestCase("\n", null)]
	[TestCase("\r\n\r\n", "\n\n")]
	public async Task TestLineEndingAnalyzer(string input, string? output)
	{
		var fixer = new LineEndingAnalyzer();

		var result = await fixer.AnalyzeAsync(input, CancellationToken.None);

		Assert.That(result.FixedString, Is.EqualTo(output));
		Assert.That(result.ShouldFix, Is.EqualTo(output != null));
	}
}
