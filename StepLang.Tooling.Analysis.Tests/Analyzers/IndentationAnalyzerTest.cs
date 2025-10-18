using StepLang.Tooling.Analysis.Analyzers;

namespace StepLang.Tooling.Analysis.Tests.Analyzers;

public class IndentationAnalyzerTest
{
	[TestCase("a", null)]
	[TestCase("  a", "a")]
	[TestCase("{\r\n  a\r\n}", "{\r\n\ta\r\n}")]
	[TestCase("{\n  a\n}", "{\n\ta\n}")]
	[TestCase("{\n\ta\n}\n", null)]
	[TestCase("{\r\n\ta\n}\r\n", null)]
	[TestCase("  }\r\n}\r\n", "}\r\n}\r\n")]
	[TestCase("\r\n\r\n", null)]
	[TestCase("   \r\n\t\r\n", "\r\n\r\n")]
	[TestCase("{\n\t\n\n}\n", "{\n\n\n}\n")]
	public async Task TestIndentationAnalyzer(string input, string? output)
	{
		var fixer = new IndentationAnalyzer();

		var result = await fixer.AnalyzeAsync(input, CancellationToken.None);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.FixedString, Is.EqualTo(output));
			Assert.That(result.ShouldFix, Is.EqualTo(output != null));
		}
	}
}
