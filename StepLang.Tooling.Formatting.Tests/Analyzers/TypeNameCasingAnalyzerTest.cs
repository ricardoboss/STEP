using StepLang.Tooling.Formatting.Analyzers;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tooling.Formatting.Tests.Analyzers;

[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes")]
public class TypeNameCasingAnalyzerTest
{
	[Theory]
	[InlineData("a", "a")]
	[InlineData(" String", " string")]
	[InlineData("NUMBER", "number")]
	[InlineData("bOOl ", "bool ")]
	[InlineData("\"Don't fix within String\"", "\"Don't fix within String\"")]
	[InlineData("\"Nested \\\"Bool\\\" String\"", "\"Nested \\\"Bool\\\" String\"")]
	public async Task TestTypeNameCasingAnalyzer(string input, string output)
	{
		var fixer = new TypeNameCasingAnalyzer();

		var result = await fixer.AnalyzeAsync(input, CancellationToken.None);

		Assert.Equal(output, result.FixedString);
		Assert.Equal(!string.Equals(input, output, StringComparison.Ordinal), result.FixRequired);
	}
}
