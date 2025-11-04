using StepLang.Diagnostics;
using StepLang.Parsing;
using StepLang.Parsing.Nodes.Statements;
using StepLang.Tokenizing;

namespace StepLang.Tests.Parsing;

public class ParserTest
{
	[Test]
	public void TestParseIdentifierUsageThrowForIncompleteStatement()
	{
		// Arrange
		const string source = "identifier";
		var diagnostics = new DiagnosticCollection();
		var tokens = source.AsTokens(diagnostics);

		var parser = new Parser(tokens, diagnostics);

		// Act
		var root = parser.ParseRoot();

		// Assert
		var errorStatement = AssertIsType<ErrorStatementNode>(root.Body.FirstOrDefault());
		using (Assert.EnterMultipleScope())
		{
			Assert.That(errorStatement.Description, Is.EqualTo("Unexpected end of tokens"));
			Assert.That(errorStatement.Tokens.FirstOrDefault()?.Type, Is.EqualTo(TokenType.Identifier));
		}
	}

	private static T AssertIsType<T>(object? value)
	{
		Assert.That(value, Is.TypeOf<T>());
		return (T)value!;
	}
}
