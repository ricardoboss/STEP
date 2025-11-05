using StepLang.Diagnostics;
using StepLang.Tokenizing;

namespace StepLang.Tests.Tokenizing;

public class TokenizerTest
{
	[Test]
	public void TestTokenizeLiteralString()
	{
		const string source = "\"abc\"";

		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToArray();

		Assert.That(tokens, Has.Length.EqualTo(2));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.LiteralString));
			Assert.That(tokens[0].Value, Is.EqualTo(source));
			Assert.That(tokens[0].StringValue, Is.EqualTo("abc"));
			Assert.That(tokens[0].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[0].Location.Column, Is.EqualTo(1));
			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokens[1].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[1].Location.Column, Is.EqualTo(source.Length + 1));
		}

		Assert.That(diagnostics, Is.Empty);
	}

	[Test]
	public void TestTokenizeLiteralStringWithEscapedChars()
	{
		const string source = "\"\\n\"";

		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToArray();

		Assert.That(tokens, Has.Length.EqualTo(2));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.LiteralString));
			Assert.That(tokens[0].StringValue, Is.EqualTo("\n"));
			Assert.That(tokens[0].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[0].Location.Column, Is.EqualTo(1));
			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokens[1].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[1].Location.Column, Is.EqualTo(source.Length + 1));
		}

		Assert.That(diagnostics, Is.Empty);
	}

	[Test]
	public void TestTokenizeLiteralNumber()
	{
		const string source = "123";

		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToArray();

		Assert.That(tokens, Has.Length.EqualTo(2));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.LiteralNumber));
			Assert.That(tokens[0].Value, Is.EqualTo("123"));
			Assert.That(tokens[0].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[0].Location.Column, Is.EqualTo(1));
			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokens[1].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[1].Location.Column, Is.EqualTo(source.Length + 1));
		}

		Assert.That(diagnostics, Is.Empty);
	}

	[Test]
	public void TestTokenizeLiteralNegativeNumber()
	{
		const string source = "-123";

		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToArray();

		Assert.That(tokens, Has.Length.EqualTo(2));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.LiteralNumber));
			Assert.That(tokens[0].Value, Is.EqualTo("-123"));
			Assert.That(tokens[0].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[0].Location.Column, Is.EqualTo(1));
			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokens[1].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[1].Location.Column, Is.EqualTo(source.Length + 1));
		}

		Assert.That(diagnostics, Is.Empty);
	}

	[Test]
	public void TestTokenizeLiteralNegativeNumberWithDecimals()
	{
		const string source = "-1.23";

		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToArray();

		Assert.That(tokens, Has.Length.EqualTo(2));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.LiteralNumber));
			Assert.That(tokens[0].Value, Is.EqualTo("-1.23"));
			Assert.That(tokens[0].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[0].Location.Column, Is.EqualTo(1));
			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokens[1].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[1].Location.Column, Is.EqualTo(source.Length + 1));
		}

		Assert.That(diagnostics, Is.Empty);
	}

	[Test]
	public void TestTokenizeLiteralStringWithSpaces()
	{
		const string source = "\"abc def\"";

		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToArray();

		Assert.That(tokens, Has.Length.EqualTo(2));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.LiteralString));
			Assert.That(tokens[0].Value, Is.EqualTo(source));
			Assert.That(tokens[0].StringValue, Is.EqualTo("abc def"));
			Assert.That(tokens[0].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[0].Location.Column, Is.EqualTo(1));
			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokens[1].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[1].Location.Column, Is.EqualTo(source.Length + 1));
		}

		Assert.That(diagnostics, Is.Empty);
	}

	[TestCase("\"abc\\\"def\"", "abc\"def")]
	public void TestTokenizeLiteralStringWithEscapedQuotes(string source, string expected)
	{
		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToArray();

		Assert.That(tokens, Has.Length.EqualTo(2));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.LiteralString));
			Assert.That(tokens[0].StringValue, Is.EqualTo(expected));
			Assert.That(tokens[0].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[0].Location.Column, Is.EqualTo(1));
			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokens[1].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[1].Location.Column, Is.EqualTo(source.Length + 1));
		}

		Assert.That(diagnostics, Is.Empty);
	}

	[TestCase("string")]
	[TestCase("number")]
	[TestCase("bool")]
	[TestCase("function")]
	public void TestTokenizeKnownType(string source)
	{
		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToList();

		Assert.That(tokens, Has.Count.EqualTo(2));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.TypeName));
			Assert.That(tokens[0].Value, Is.EqualTo(source));
			Assert.That(tokens[0].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[0].Location.Column, Is.EqualTo(1));
			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokens[1].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[1].Location.Column, Is.EqualTo(source.Length + 1));
		}

		Assert.That(diagnostics, Is.Empty);
	}

	[TestCase("if", TokenType.IfKeyword)]
	[TestCase("else", TokenType.ElseKeyword)]
	[TestCase("while", TokenType.WhileKeyword)]
	[TestCase("break", TokenType.BreakKeyword)]
	[TestCase("continue", TokenType.ContinueKeyword)]
	public void TestTokenizeKeyword(string source, TokenType type)
	{
		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToArray();

		Assert.That(tokens, Has.Length.EqualTo(2));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(type));
			Assert.That(tokens[0].Value, Is.EqualTo(source));
			Assert.That(tokens[0].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[0].Location.Column, Is.EqualTo(1));
			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokens[1].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[1].Location.Column, Is.EqualTo(source.Length + 1));
		}

		Assert.That(diagnostics, Is.Empty);
	}

	[Test]
	public void TestTokenizeMultipleTokens()
	{
		const string source = "number identifier = 1";

		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToList();

		Assert.That(tokens, Has.Count.EqualTo(8));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.TypeName));
			Assert.That(tokens[0].Value, Is.EqualTo("number"));
			Assert.That(tokens[0].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[0].Location.Column, Is.EqualTo(1));
			Assert.That(tokens[0].Location.Length, Is.EqualTo(6));

			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokens[1].Value, Is.EqualTo(" "));
			Assert.That(tokens[1].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[1].Location.Column, Is.EqualTo(7));
			Assert.That(tokens[1].Location.Length, Is.EqualTo(1));

			Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Identifier));
			Assert.That(tokens[2].Value, Is.EqualTo("identifier"));
			Assert.That(tokens[2].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[2].Location.Column, Is.EqualTo(8));
			Assert.That(tokens[2].Location.Length, Is.EqualTo(10));

			Assert.That(tokens[3].Type, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokens[3].Value, Is.EqualTo(" "));
			Assert.That(tokens[3].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[3].Location.Column, Is.EqualTo(18));
			Assert.That(tokens[3].Location.Length, Is.EqualTo(1));

			Assert.That(tokens[4].Type, Is.EqualTo(TokenType.EqualsSymbol));
			Assert.That(tokens[4].Value, Is.EqualTo("="));
			Assert.That(tokens[4].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[4].Location.Column, Is.EqualTo(19));
		}

		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[4].Location.Column, Is.EqualTo(tokens[3].Location.Column + tokens[3].Location.Length));
			Assert.That(tokens[4].Location.Length, Is.EqualTo(1));

			Assert.That(tokens[5].Type, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokens[5].Value, Is.EqualTo(" "));
			Assert.That(tokens[5].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[5].Location.Column, Is.EqualTo(20));
			Assert.That(tokens[5].Location.Length, Is.EqualTo(1));

			Assert.That(tokens[6].Type, Is.EqualTo(TokenType.LiteralNumber));
			Assert.That(tokens[6].Value, Is.EqualTo("1"));
			Assert.That(tokens[6].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[6].Location.Column, Is.EqualTo(21));
			Assert.That(tokens[6].Location.Length, Is.EqualTo(1));

			Assert.That(tokens[7].Type, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokens[7].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[7].Location.Column, Is.EqualTo(22));
			Assert.That(tokens[7].Location.Length, Is.Zero);

			Assert.That(diagnostics, Is.Empty, TestHelper.StringifyDiagnostics(diagnostics));
		}
	}

	[Test]
	public void TestTokenizeKeywords()
	{
		const string source = "if (true)";

		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToList();

		Assert.That(tokens, Has.Count.EqualTo(6));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.IfKeyword));
			Assert.That(tokens[0].Value, Is.EqualTo("if"));
			Assert.That(tokens[0].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[0].Location.Column, Is.EqualTo(1));
			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokens[1].Value, Is.EqualTo(" "));
			Assert.That(tokens[1].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[1].Location.Column, Is.EqualTo(3));
			Assert.That(tokens[2].Type, Is.EqualTo(TokenType.OpeningParentheses));
			Assert.That(tokens[2].Value, Is.EqualTo("("));
			Assert.That(tokens[2].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[2].Location.Column, Is.EqualTo(4));
			Assert.That(tokens[3].Type, Is.EqualTo(TokenType.LiteralBoolean));
			Assert.That(tokens[3].Value, Is.EqualTo("true"));
			Assert.That(tokens[3].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[3].Location.Column, Is.EqualTo(5));
			Assert.That(tokens[4].Type, Is.EqualTo(TokenType.ClosingParentheses));
			Assert.That(tokens[4].Value, Is.EqualTo(")"));
			Assert.That(tokens[4].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[4].Location.Column, Is.EqualTo(9));
			Assert.That(tokens[5].Type, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(tokens[5].Location.Line, Is.EqualTo(1));
			Assert.That(tokens[5].Location.Column, Is.EqualTo(10));
		}

		Assert.That(diagnostics, Is.Empty);
	}

	[Test]
	public void TestTokenizeFunctionCall()
	{
		const string source = "print(\"hello\")";

		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToList();

		Assert.That(tokens, Has.Count.EqualTo(5));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
			Assert.That(tokens[0].Value, Is.EqualTo("print"));
			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.OpeningParentheses));
			Assert.That(tokens[1].Value, Is.EqualTo("("));
			Assert.That(tokens[2].Type, Is.EqualTo(TokenType.LiteralString));
			Assert.That(tokens[2].Value, Is.EqualTo("\"hello\""));
			Assert.That(tokens[3].Type, Is.EqualTo(TokenType.ClosingParentheses));
			Assert.That(tokens[3].Value, Is.EqualTo(")"));
			Assert.That(tokens[4].Type, Is.EqualTo(TokenType.EndOfFile));
		}

		Assert.That(diagnostics, Is.Empty);
	}

	[Test]
	public void TestTokenizeLineComment()
	{
		const string source = "f(variable) // this is a comment\nprintln(\"text\") // more comments";

		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToList();

		Assert.That(tokens, Has.Count.EqualTo(14));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
			Assert.That(tokens[0].Value, Is.EqualTo("f"));
			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.OpeningParentheses));
			Assert.That(tokens[1].Value, Is.EqualTo("("));
			Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Identifier));
			Assert.That(tokens[2].Value, Is.EqualTo("variable"));
			Assert.That(tokens[3].Type, Is.EqualTo(TokenType.ClosingParentheses));
			Assert.That(tokens[3].Value, Is.EqualTo(")"));
			Assert.That(tokens[4].Type, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokens[4].Value, Is.EqualTo(" "));
			Assert.That(tokens[5].Type, Is.EqualTo(TokenType.LineComment));
			Assert.That(tokens[5].Value, Is.EqualTo("// this is a comment"));
			Assert.That(tokens[6].Type, Is.EqualTo(TokenType.NewLine));
			Assert.That(tokens[6].Value, Is.EqualTo("\n"));
			Assert.That(tokens[7].Type, Is.EqualTo(TokenType.Identifier));
			Assert.That(tokens[7].Value, Is.EqualTo("println"));
			Assert.That(tokens[8].Type, Is.EqualTo(TokenType.OpeningParentheses));
			Assert.That(tokens[8].Value, Is.EqualTo("("));
			Assert.That(tokens[9].Type, Is.EqualTo(TokenType.LiteralString));
			Assert.That(tokens[9].Value, Is.EqualTo("\"text\""));
			Assert.That(tokens[10].Type, Is.EqualTo(TokenType.ClosingParentheses));
			Assert.That(tokens[10].Value, Is.EqualTo(")"));
			Assert.That(tokens[11].Type, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokens[11].Value, Is.EqualTo(" "));
			Assert.That(tokens[12].Type, Is.EqualTo(TokenType.LineComment));
			Assert.That(tokens[12].Value, Is.EqualTo("// more comments"));
			Assert.That(tokens[13].Type, Is.EqualTo(TokenType.EndOfFile));
		}

		Assert.That(diagnostics, Is.Empty);
	}

	[Test]
	public void TestTokenizeEmitsAnIdentifierBeforeALiteralString()
	{
		const string source = "identifier\"\"";

		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToList();

		Assert.That(tokens, Has.Count.EqualTo(3));
		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
			Assert.That(tokens[0].Value, Is.EqualTo("identifier"));
			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.LiteralString));
			Assert.That(tokens[1].Value, Is.EqualTo("\"\""));
			Assert.That(tokens[2].Type, Is.EqualTo(TokenType.EndOfFile));
		}

		Assert.That(diagnostics, Is.Empty);
	}

	[Test]
	public void TestReportsInvalidIdentifierDiagnostic()
	{
		const string source = "number a\u007fb = 1";

		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToList();

		Assert.That(tokens, Has.Count.EqualTo(8));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.TypeName));
			Assert.That(tokens[0].Value, Is.EqualTo("number"));
			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokens[1].Value, Is.EqualTo(" "));
			Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Error));
			Assert.That(tokens[2].Value, Is.EqualTo("a\u007fb"));
			Assert.That(tokens[3].Type, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokens[3].Value, Is.EqualTo(" "));
			Assert.That(tokens[4].Type, Is.EqualTo(TokenType.EqualsSymbol));
			Assert.That(tokens[4].Value, Is.EqualTo("="));
			Assert.That(tokens[5].Type, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokens[5].Value, Is.EqualTo(" "));
			Assert.That(tokens[6].Type, Is.EqualTo(TokenType.LiteralNumber));
			Assert.That(tokens[6].Value, Is.EqualTo("1"));
			Assert.That(tokens[7].Type, Is.EqualTo(TokenType.EndOfFile));
		}

		var diagnostic = diagnostics.Single();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(diagnostic.Severity, Is.EqualTo(Severity.Error));
			Assert.That(diagnostic.Message, Is.EqualTo("Invalid identifier"));
			Assert.That(diagnostic.Code, Is.EqualTo("TOK001"));
			Assert.That(diagnostic.Kind, Is.Null);
			Assert.That(diagnostic.Area, Is.EqualTo(DiagnosticArea.Tokenizing));
			Assert.That(diagnostic.Token, Is.EqualTo(tokens[2]));
			Assert.That(diagnostic.Location?.Line, Is.EqualTo(1));
			Assert.That(diagnostic.Location?.Column, Is.EqualTo(8));
			Assert.That(diagnostic.Location?.Length, Is.EqualTo(3));
			Assert.That(diagnostic.RelatedTokens, Is.Null);
		}
	}

	[Test]
	public void TestReportsUnterminatedStringDiagnostic()
	{
		const string source = "\"string";

		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToList();

		Assert.That(tokens, Has.Count.EqualTo(2));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Error));
			Assert.That(tokens[0].Value, Is.EqualTo("\"string"));
			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.EndOfFile));
		}

		var diagnostic = diagnostics.Single();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(diagnostic.Severity, Is.EqualTo(Severity.Error));
			Assert.That(diagnostic.Message, Is.EqualTo("Unterminated string"));
			Assert.That(diagnostic.Code, Is.EqualTo("TOK002"));
			Assert.That(diagnostic.Kind, Is.Null);
			Assert.That(diagnostic.Area, Is.EqualTo(DiagnosticArea.Tokenizing));
			Assert.That(diagnostic.Token, Is.EqualTo(tokens[0]));
			Assert.That(diagnostic.Location?.Line, Is.EqualTo(1));
			Assert.That(diagnostic.Location?.Column, Is.EqualTo(1));
			Assert.That(diagnostic.Location?.Length, Is.EqualTo(source.Length));
			Assert.That(diagnostic.RelatedTokens, Is.Null);
		}
	}

	[Test]
	public void TestTokenizeKeywordInIdentifier()
	{
		// identifier "ifempty" contains the keyword "if"
		const string source = "println(ifempty(\"abc\", \"b\"))";

		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToList();

		Assert.That(tokens, Has.Count.EqualTo(11));
		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
			Assert.That(tokens[0].Value, Is.EqualTo("println"));
			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.OpeningParentheses));
			Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Identifier));
			Assert.That(tokens[2].Value, Is.EqualTo("ifempty"));
			Assert.That(tokens[3].Type, Is.EqualTo(TokenType.OpeningParentheses));
			Assert.That(tokens[4].Type, Is.EqualTo(TokenType.LiteralString));
			Assert.That(tokens[4].Value, Is.EqualTo("\"abc\""));
			Assert.That(tokens[5].Type, Is.EqualTo(TokenType.CommaSymbol));
			Assert.That(tokens[6].Type, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokens[7].Type, Is.EqualTo(TokenType.LiteralString));
			Assert.That(tokens[7].Value, Is.EqualTo("\"b\""));
			Assert.That(tokens[8].Type, Is.EqualTo(TokenType.ClosingParentheses));
			Assert.That(tokens[9].Type, Is.EqualTo(TokenType.ClosingParentheses));
			Assert.That(tokens[10].Type, Is.EqualTo(TokenType.EndOfFile));
		}
	}

	[Test]
	public void TestTokenizeLineCommentAfterTokens()
	{
		const string source = "a = b// more comments";

		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToList();

		Assert.That(tokens, Has.Count.EqualTo(7));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
			Assert.That(tokens[0].Value, Is.EqualTo("a"));
			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokens[1].Value, Is.EqualTo(" "));
			Assert.That(tokens[2].Type, Is.EqualTo(TokenType.EqualsSymbol));
			Assert.That(tokens[2].Value, Is.EqualTo("="));
			Assert.That(tokens[3].Type, Is.EqualTo(TokenType.Whitespace));
			Assert.That(tokens[3].Value, Is.EqualTo(" "));
			Assert.That(tokens[4].Type, Is.EqualTo(TokenType.Identifier));
			Assert.That(tokens[4].Value, Is.EqualTo("b"));
			Assert.That(tokens[5].Type, Is.EqualTo(TokenType.LineComment));
			Assert.That(tokens[5].Value, Is.EqualTo("// more comments"));
			Assert.That(tokens[6].Type, Is.EqualTo(TokenType.EndOfFile));
		}

		Assert.That(diagnostics, Is.Empty);
	}

	[Test]
	public void TestParsesStringsWithUnescapedControlCharacters()
	{
		const string source = "\"abc\r";

		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToList();

		Assert.That(tokens, Has.Count.EqualTo(3));
		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Error));
			Assert.That(tokens[0].Value, Is.EqualTo("\"abc"));
			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Error));
			Assert.That(tokens[1].Value, Is.EqualTo("\r"));
			Assert.That(tokens[2].Type, Is.EqualTo(TokenType.EndOfFile));
		}

		Assert.That(diagnostics, Has.Count.EqualTo(2));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(diagnostics[0].Area, Is.EqualTo(DiagnosticArea.Tokenizing));
			Assert.That(diagnostics[0].Code, Is.EqualTo("TOK002"));
			Assert.That(diagnostics[0].Column, Is.EqualTo(1));
			Assert.That(diagnostics[0].Length, Is.EqualTo(4));
			Assert.That(diagnostics[0].Line, Is.EqualTo(1));
			Assert.That(diagnostics[0].Message, Is.EqualTo("Unterminated string"));
			Assert.That(diagnostics[0].Severity, Is.EqualTo(Severity.Error));

			Assert.That(diagnostics[1].Area, Is.EqualTo(DiagnosticArea.Tokenizing));
			Assert.That(diagnostics[1].Code, Is.EqualTo("TOK002"));
			Assert.That(diagnostics[1].Column, Is.EqualTo(5));
			Assert.That(diagnostics[1].Length, Is.EqualTo(1));
			Assert.That(diagnostics[1].Line, Is.EqualTo(1));
			Assert.That(diagnostics[1].Message, Is.EqualTo("Unescaped control character"));
			Assert.That(diagnostics[1].Severity, Is.EqualTo(Severity.Error));
		}
	}

	[Test]
	public void TestReturnsNewLineAfterUnterminatedString()
	{
		const string source = "\"abc\n";

		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToList();

		Assert.That(tokens, Has.Count.EqualTo(3));
		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Error));
			Assert.That(tokens[0].Value, Is.EqualTo("\"abc"));
			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.NewLine));
			Assert.That(tokens[1].Value, Is.EqualTo("\n"));
			Assert.That(tokens[2].Type, Is.EqualTo(TokenType.EndOfFile));
		}

		var diagnostic = diagnostics.Single();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(diagnostic.Area, Is.EqualTo(DiagnosticArea.Tokenizing));
			Assert.That(diagnostic.Code, Is.EqualTo("TOK002"));
			Assert.That(diagnostic.Column, Is.EqualTo(1));
			Assert.That(diagnostic.Length, Is.EqualTo(4));
			Assert.That(diagnostic.Line, Is.EqualTo(1));
			Assert.That(diagnostic.Message, Is.EqualTo("Unterminated string"));
			Assert.That(diagnostic.Severity, Is.EqualTo(Severity.Error));
		}
	}

	[Test]
	public void TestSkipsCarriageReturns()
	{
		const string source = "\r\n";

		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToList();

		Assert.That(tokens, Has.Count.EqualTo(2));
		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.NewLine));
			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(diagnostics, Is.Empty, TestHelper.StringifyDiagnostics(diagnostics));
		}
	}

	[Test]
	public void TestTokenizeSimpleAssignment()
	{
		const string source = "n = a - b";

		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToList();

		Assert.That(tokens, Has.Count.EqualTo(10));
		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
			Assert.That(tokens[2].Type, Is.EqualTo(TokenType.EqualsSymbol));
			Assert.That(tokens[4].Type, Is.EqualTo(TokenType.Identifier));
			Assert.That(tokens[6].Type, Is.EqualTo(TokenType.MinusSymbol));
			Assert.That(tokens[8].Type, Is.EqualTo(TokenType.Identifier));
			Assert.That(diagnostics, Is.Empty, TestHelper.StringifyDiagnostics(diagnostics));
		}
	}

	[Test]
	public void TestTokenizesFunctionCallParameterWithNegateExpression()
	{
		const string source = "foo(-a)";
		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(source, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken).ToList();

		Assert.That(tokens, Has.Count.EqualTo(6));
		using (Assert.EnterMultipleScope())
		{
			Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
			Assert.That(tokens[1].Type, Is.EqualTo(TokenType.OpeningParentheses));
			Assert.That(tokens[2].Type, Is.EqualTo(TokenType.MinusSymbol));
			Assert.That(tokens[3].Type, Is.EqualTo(TokenType.Identifier));
			Assert.That(tokens[4].Type, Is.EqualTo(TokenType.ClosingParentheses));
			Assert.That(tokens[5].Type, Is.EqualTo(TokenType.EndOfFile));
			Assert.That(diagnostics, Is.Empty, TestHelper.StringifyDiagnostics(diagnostics));
		}
	}
}
