using StepLang.Tooling.Analysis.Analyzers;

namespace StepLang.Tooling.Analysis.Tests.Analyzers;

public class KeywordCasingAnalyzerTest
{
	[TestCase("a", null)]
	[TestCase(" Import", " import")]
	[TestCase("IF (TRUE)", "if (TRUE)")]
	[TestCase("\"don't fix IF within a string\"", null)]
	[TestCase("IF (\"don't fix IF within a string\")", "if (\"don't fix IF within a string\")")]
	[TestCase("\"escaped \\\"IMPORT\\\" IF\"", null)]
	[TestCase("Break", "break")]
	[TestCase("Continue", "continue")]
	[TestCase("Else", "else")]
	[TestCase("WHILe", "while")]
	[TestCase("rETURN", "return")]
	public async Task TestKeywordCasingAnalyzer(string input, string? output)
	{
		var fixer = new KeywordCasingAnalyzer();

		var result = await fixer.AnalyzeAsync(input, CancellationToken.None);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.FixedString, Is.EqualTo(output));
			Assert.That(result.ShouldFix, Is.EqualTo(output != null));
		}
	}
}
