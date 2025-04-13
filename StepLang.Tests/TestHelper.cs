using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Parsing.Nodes;
using StepLang.Tokenizing;

namespace StepLang.Tests;

internal static class TestHelper
{
	public static IEnumerable<Token> AsTokens(this string code)
	{
		var tokenizer = new Tokenizer(code, []);

		return tokenizer.Tokenize(TestContext.Current.CancellationToken);
	}

	public static RootNode AsParsed(this string code)
	{
		var parser = new Parser(code.AsTokens());

		return parser.ParseRoot();
	}

	public static void Interpret(this string code)
	{
		var root = code.AsParsed();

		var interpreter = new Interpreter();

		root.Accept(interpreter);
	}
}
