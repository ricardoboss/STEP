using StepLang.Tooling.Formatting.Analyzers;

namespace StepLang.Tooling.Formatting.Tests.Analyzers;

public class IndentationAnalyzerTest
{
	[Theory]
	[InlineData("a", null)]
	[InlineData("  a", "a")]
	[InlineData("{\r\n  a\r\n}", "{\r\n\ta\r\n}")]
	[InlineData("{\n  a\n}", "{\n\ta\n}")]
	[InlineData("{\n\ta\n}\n", null)]
	[InlineData("{\r\n\ta\n}\r\n", null)]
	[InlineData("  }\r\n}\r\n", "}\r\n}\r\n")]
	[InlineData("\r\n\r\n", null)]
	[InlineData("   \r\n\t\r\n", "\r\n\r\n")]
	[InlineData("{\n\t\n\n}\n", "{\n\n\n}\n")]
	public async Task TestIndentationAnalyzer(string input, string? output)
	{
		var fixer = new IndentationAnalyzer();

		var result = await fixer.AnalyzeAsync(input, CancellationToken.None);

		Assert.Equal(output, result.FixedString);
		Assert.Equal(output != null, result.ShouldFix);
	}
}
