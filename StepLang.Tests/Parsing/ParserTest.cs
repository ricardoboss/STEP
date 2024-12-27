using StepLang.Parsing;

namespace StepLang.Tests.Parsing;

public class ParserTest
{
	[Fact]
	public void TestParseIdentifierUsageThrowForIncompleteStatement()
	{
		// Arrange
		const string source = "identifier";
		var tokens = source.AsTokens();

		var parser = new Parser(tokens);

		// Act
		var exception = Assert.Throws<UnexpectedEndOfTokensException>(() => parser.ParseIdentifierUsage());

		// Assert
		Assert.Equal("Expected a statement", exception.Message);
	}
}
