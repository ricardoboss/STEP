using StepLang.Tooling.Formatting.Analyzers;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tooling.Formatting.Tests.Analyzers;

[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes")]
public class KeywordCasingAnalyzerTest
{
	[Theory]
	[InlineData("a", "a")]
	[InlineData(" Import", " import")]
	[InlineData("IF (TRUE)", "if (TRUE)")]
	[InlineData("\"don't fix IF within a string\"", "\"don't fix IF within a string\"")]
	[InlineData("IF (\"don't fix IF within a string\")", "if (\"don't fix IF within a string\")")]
	[InlineData("\"escaped \\\"IMPORT\\\" IF\"", "\"escaped \\\"IMPORT\\\" IF\"")]
	[InlineData("Break", "break")]
	[InlineData("Continue", "continue")]
	[InlineData("Else", "else")]
	[InlineData("WHILe", "while")]
	[InlineData("rETURN", "return")]
	public async Task TestKeywordCasingAnalyzer(string input, string output)
	{
		var fixer = new KeywordCasingAnalyzer();

		var result = await fixer.AnalyzeAsync(input, CancellationToken.None);

		Assert.Equal(output, result.FixedString);
		Assert.Equal(!string.Equals(input, output, StringComparison.Ordinal), result.FixRequired);
	}
}
