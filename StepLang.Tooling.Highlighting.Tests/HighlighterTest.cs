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

		Assert.That(tokens.Count, Is.EqualTo(8));
	}

	[TestCaseSource(nameof(ExplicitlyStyledTokenTypes))]
	public void TestStyleCoversEveryTokenType(TokenType type)
	{
		var scheme = ColorScheme.Mono;
		var style = Highlighter.GetStyle(type, scheme);

		Assert.That(style.IsDefault, Is.False);
	}

	[Fact]
	public void TestHighlighterKeepsEscapesInStrings()
	{
		const string source = "string a = \"message: \\\"Hello, World!\\\"\";";

		var highlighter = new Highlighter(ColorScheme.Mono);
		var tokens = highlighter.Highlight(source).ToList();

		var literal = Assert.Single(tokens, t => t.Type == TokenType.LiteralString);

		Assert.Equal("\"message: \\\"Hello, World!\\\"\"", literal.Text);
	}

	public static IEnumerable<object[]> ExplicitlyStyledTokenTypes()
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
