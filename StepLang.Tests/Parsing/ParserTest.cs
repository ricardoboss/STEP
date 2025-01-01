using StepLang.Diagnostics;
using StepLang.Parsing;

namespace StepLang.Tests.Parsing;

public class ParserTest
{
	[Fact]
	public void TestParseIdentifierUsageThrowForIncompleteStatement()
	{
		// Arrange
		const string source = "identifier";
		var diagnostics = new DiagnosticCollection();
		var tokens = source.AsTokens(diagnostics);

		var parser = new Parser(tokens, diagnostics);

		// Act
		// TODO: rewrite using diagnostics
		var exception = Assert.Throws<UnexpectedEndOfTokensException>(() => parser.ParseIdentifierUsage());

		// Assert
		Assert.Equal("Expected a statement", exception.Message);
	}
}
