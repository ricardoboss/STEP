using StepLang.Tooling.Formatting.Analyzers;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tooling.Formatting.Tests.Analyzers;

[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes")]
public class IndentationAnalyzerTest
{
	[Theory]
	[InlineData("a", "a")]
	[InlineData("  a", "a")]
	[InlineData("{\r\n  a\r\n}", "{\r\n\ta\r\n}")]
	[InlineData("{\n  a\n}", "{\n\ta\n}")]
	[InlineData("{\n\ta\n}\n", "{\n\ta\n}\n")]
	[InlineData("{\r\n\ta\n}\r\n", "{\r\n\ta\n}\r\n")]
	[InlineData("  }\r\n}\r\n", "}\r\n}\r\n")]
	[InlineData("\r\n\r\n", "\r\n\r\n")]
	[InlineData("   \r\n\t\r\n", "\r\n\r\n")]
	[InlineData("{\n\t\n\n}\n", "{\n\n\n}\n")]
	public async Task TestIndentationAnalyzer(string input, string output)
	{
		var fixer = new IndentationAnalyzer();

		var result = await fixer.AnalyzeAsync(input, CancellationToken.None);

		Assert.Equal(output, result.FixedString);
		Assert.Equal(!string.Equals(input, output, StringComparison.Ordinal), result.FixRequired);
	}
}
