using StepLang.Tooling.Analysis.Analyzers;

namespace StepLang.Tooling.Analysis.Tests.Analyzers;

public class TypeNameCasingAnalyzerTest
{
	[TestCase("a", null)]
	[TestCase(" String", " string")]
	[TestCase("NUMBER", "number")]
	[TestCase("bOOl ", "bool ")]
	[TestCase("\"Don't fix within String\"", null)]
	[TestCase("\"Nested \\\"Bool\\\" String\"", null)]
	public async Task TestTypeNameCasingAnalyzer(string input, string? output)
	{
		var fixer = new TypeNameCasingAnalyzer();

		var result = await fixer.AnalyzeAsync(input, CancellationToken.None);

		Assert.That(result.FixedString, Is.EqualTo(output));
		Assert.That(result.ShouldFix, Is.EqualTo(output != null));
	}
}
