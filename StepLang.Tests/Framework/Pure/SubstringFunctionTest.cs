using StepLang.Diagnostics;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Tests.Framework.Pure;

public class SubstringFunctionTest
{
	[TestCase("substring(\"abc\", 0, 1)", "a")]
	[TestCase("substring(\"Hello, world!\", 0, 5)", "Hello")]
	[TestCase("substring(\"Hello, world!\", 7, 5)", "world")]
	[TestCase("substring(\"Hello, world!\", 7, 100)", "world!")]
	[TestCase("substring(\"Hello, world!\", 7, 0)", "")]
	[TestCase("substring(\"Hello, world!\", 7, -1)", "")]
	[TestCase("substring(\"Hello, world!\", 7, 1.5)", "wo")]
	[TestCase("substring(\"Hello, world!\", 7.5, 1)", "o")]
	[TestCase("substring(\"Hello, world!\", 13, 0)", "")]
	[TestCase("substring(\"Hello, world!\", 13, 1)", "")]
	[TestCase("substring(\"Hello, world!\", 100, 5)", "")]
	[TestCase("substring(\"Hello, world!\", 12, 1)", "!")]
	[TestCase("substring(\"Hello, world!\", -1, 3)", "!")]
	[TestCase("substring(\"Hello, world!\", -2, 3)", "d!")]
	[TestCase("substring(\"Hello, world!\", -3, 3)", "ld!")]
	[TestCase("substring(\"Hello, world!\", -4, 3)", "rld")]
	[TestCase("substring(\"Hello, world!\", -13, 5)", "Hello")]
	[TestCase("substring(\"Hello, world!\", -14, 5)", "")]
	[TestCase("substring(\"Hello, world!\", -100, 5)", "")]
	public void TestSubstring(string code, string result)
	{
		var diagnostics = new DiagnosticCollection();

		var tokenizer = new Tokenizer($"print({code})", diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken);

		var parser = new Parser(tokens, diagnostics);
		var root = parser.ParseRoot();

		using var output = new StringWriter();
		var interpreter = new Interpreter(output);
		root.Accept(interpreter);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(output.ToString(), Is.EqualTo(result));
			Assert.That(diagnostics, Is.Empty, TestHelper.StringifyDiagnostics(diagnostics));
		}
	}
}
