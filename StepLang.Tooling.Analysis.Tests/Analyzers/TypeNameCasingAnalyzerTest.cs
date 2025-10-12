using StepLang.Tooling.Analysis.Analyzers;

namespace StepLang.Tooling.Analysis.Tests.Analyzers;

public class TypeNameCasingAnalyzerTest
{
	[Theory]
	[InlineData("a", null)]
	[InlineData(" String", " string")]
	[InlineData("NUMBER", "number")]
	[InlineData("bOOl ", "bool ")]
	[InlineData("\"Don't fix within String\"", null)]
	[InlineData("\"Nested \\\"Bool\\\" String\"", null)]
	public async Task TestTypeNameCasingAnalyzer(string input, string? output)
	{
		var fixer = new TypeNameCasingAnalyzer();

		var result = await fixer.AnalyzeAsync(input, CancellationToken.None);

		Assert.Equal(output, result.FixedString);
		Assert.Equal(output != null, result.ShouldFix);
	}
}
