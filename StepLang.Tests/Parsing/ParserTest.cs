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
			Assert.That(errorStatement.Tokens.FirstOrDefault()?.Type, Is.EqualTo(TokenType.EndOfFile));
		}
	}

	[Test]
	public void TestParsesSimpleIncrement()
	{
		const string source = "i++";
		var tokens = source.AsTokens();
		var diagnostics = new DiagnosticCollection();
		var parser = new Parser(tokens, diagnostics);
		var root = parser.ParseRoot();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(root.Body, Has.Count.EqualTo(1));
			Assert.That(diagnostics, Is.Empty, TestHelper.StringifyDiagnostics(diagnostics));
		}
	}

	[Test]
	public void TestParsesSimpleDecrement()
	{
		const string source = "i--";
		var tokens = source.AsTokens();
		var diagnostics = new DiagnosticCollection();
		var parser = new Parser(tokens, diagnostics);
		var root = parser.ParseRoot();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(root.Body, Has.Count.EqualTo(1));
			Assert.That(diagnostics, Is.Empty, TestHelper.StringifyDiagnostics(diagnostics));
		}
	}

	[Test]
	public void TestParsesAssignmentWithSubtractionExpression()
	{
		const string source = "n = a - b";
		var diagnostics = new DiagnosticCollection();
		var tokens = source.AsTokens(diagnostics);
		var parser = new Parser(tokens, diagnostics);
		var root = parser.ParseRoot();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(root.Body, Has.Count.EqualTo(1));
			Assert.That(diagnostics, Is.Empty, TestHelper.StringifyDiagnostics(diagnostics));
		}
	}

	[Test]
	public void TestParsesNegateExpressionAsFunctionParameter()
	{
		const string source = "clamp(1, 5, -a)";
		var diagnostics = new DiagnosticCollection();
		var tokens = source.AsTokens(diagnostics);
		var parser = new Parser(tokens, diagnostics);
		var root = parser.ParseRoot();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(root.Body, Has.Count.EqualTo(1));
			Assert.That(diagnostics, Is.Empty, TestHelper.StringifyDiagnostics(diagnostics));
		}
	}

	private static T AssertIsType<T>(object? value)
	{
		Assert.That(value, Is.TypeOf<T>());
		return (T)value!;
	}
}
