using StepLang.Tokenizing;

namespace StepLang.Tooling.Highlighting.Tests;

public class HighlighterTest
{
	[Test]
	public void TestHighlighterHighlightsSimpleTokens()
	{
		const string source = "number a = 1";

		var highlighter = new Highlighter(ColorScheme.Mono);
		var tokens = highlighter.Highlight(source).ToList();

		Assert.That(tokens, Has.Count.EqualTo(8));
	}

	[Test]
	public void TestHighlighterHighlightsBackslashesInStringsCorrectly()
	{
		const string source = """
		                      string a = "message: \"Hello, World!\""
		                      """;

		var highlighter = new Highlighter(ColorScheme.Mono);
		var tokens = highlighter.Highlight(source).ToList();

		Assert.That(tokens, Has.Count.EqualTo(8));
		Assert.That(tokens[6].Text, Is.EqualTo("\"message: \\\"Hello, World!\\\"\""));
	}

	[TestCaseSource(nameof(ExplicitlyStyledTokenTypes))]
	public void TestStyleCoversEveryTokenType(TokenType type)
	{
		var scheme = ColorScheme.Mono;
		var style = Highlighter.GetStyle(type, scheme);

		Assert.That(style.IsDefault, Is.False);
	}

	private static IEnumerable<object[]> ExplicitlyStyledTokenTypes()
	{
		return Enum.GetValues<TokenType>()
			.Except([
				TokenType.Whitespace,
				TokenType.NewLine,
				TokenType.EndOfFile,
			])
			.Select(t => new object[] { t });
	}
}
