using StepLang.Tooling.Analysis.Analyzers;

namespace StepLang.Tooling.Analysis.Tests.Analyzers;

public class KeywordCasingAnalyzerTest
{
	[Theory]
	[InlineData("a", null)]
	[InlineData(" Import", " import")]
	[InlineData("IF (TRUE)", "if (TRUE)")]
	[InlineData("\"don't fix IF within a string\"", null)]
	[InlineData("IF (\"don't fix IF within a string\")", "if (\"don't fix IF within a string\")")]
	[InlineData("\"escaped \\\"IMPORT\\\" IF\"", null)]
	[InlineData("Break", "break")]
	[InlineData("Continue", "continue")]
	[InlineData("Else", "else")]
	[InlineData("WHILe", "while")]
	[InlineData("rETURN", "return")]
	public async Task TestKeywordCasingAnalyzer(string input, string? output)
	{
		var fixer = new KeywordCasingAnalyzer();

		var result = await fixer.AnalyzeAsync(input, CancellationToken.None);

		Assert.Equal(output, result.FixedString);
		Assert.Equal(output != null, result.ShouldFix);
	}
}
