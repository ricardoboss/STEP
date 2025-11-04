using StepLang.Diagnostics;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Parsing.Nodes;
using StepLang.Tokenizing;
using System.Text;

namespace StepLang.Tests;

internal static class TestHelper
{
	public static IEnumerable<Token> AsTokens(this string code, DiagnosticCollection? diagnostics = null)
	{
		var tokenizer = new Tokenizer(code, diagnostics ?? []);

		return tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken);
	}

	public static RootNode AsParsed(this string code, DiagnosticCollection? diagnostics = null)
	{
		diagnostics ??= [];
		var parser = new Parser(code.AsTokens(diagnostics), diagnostics);

		var root = parser.ParseRoot();

		if (diagnostics is { ContainsErrors: true })
		{
			var sb = new StringBuilder();
			sb.AppendLine("Parsing code caused errors:");
			foreach (var d in diagnostics.Errors)
			{
				sb.Append("- ");
				sb.Append(d.Message);
				sb.AppendLine();
			}

			throw new ArgumentException(sb.ToString(), nameof(code));
		}

		return root;
	}

	public static void Interpret(this string code, DiagnosticCollection? diagnostics = null)
	{
		diagnostics ??= [];

		var root = code.AsParsed(diagnostics);

		var interpreter = new Interpreter(diagnostics: diagnostics);

		root.Accept(interpreter);
	}
}
