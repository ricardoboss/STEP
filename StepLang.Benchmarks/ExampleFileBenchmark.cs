using BenchmarkDotNet.Attributes;
using StepLang.Diagnostics;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Parsing.Nodes;
using StepLang.Tokenizing;

namespace StepLang.Benchmarks;

public class ExampleFileBenchmark
{
	[ParamsSource(nameof(ExampleFileNames))]
	public string ExampleFileName { get; set; } = null!;

	public static IEnumerable<string> ExampleFileNames
	{
		get
		{
			// yield return "assignment.step";
			// yield return "clone.step";
			// yield return "conversions.step";
			// yield return "docs-substring-behaviour.step";
			// yield return "docs-toJson-behaviour.step";
			// yield return "env.step";
			yield return "expressions.step";
			// yield return "functions.step";
			// yield return "imports.step";
			// yield return "isset.step";
			yield return "json.step";
			// yield return "lists.step";
			yield return "looping.step";
			// yield return "maps.step";
			yield return "math.step";
			// yield return "nullable.step";
			// yield return "range.step";
			// yield return "recursive.step";
			// yield return "step-by.step";
			yield return "strings.step";
			// yield return "time.step";
			// yield return "zipped.step";
		}
	}

	[Benchmark]
	public IReadOnlyList<Token> Tokenize()
	{
		var exampleFile = new FileInfo(ExampleFileName);

		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(exampleFile, diagnostics);
		var tokens = tokenizer.Tokenize().ToList();

		if (diagnostics.ContainsErrors)
			throw new InvalidOperationException("Example file contains tokenizer errors");

		return tokens;
	}

	[Benchmark]
	public RootNode Parse()
	{
		var diagnostics = new DiagnosticCollection();

		var tokens = Tokenize();

		var parser = new Parser(tokens, diagnostics);
		var root = parser.ParseRoot();

		if (diagnostics.ContainsErrors)
			throw new InvalidOperationException("Example file contains parser errors");

		return root;
	}

	[Benchmark]
	public void Interpret()
	{
		var diagnostics = new DiagnosticCollection();

		var root = Parse();

		var interpreter = new Interpreter(null, null, null, diagnostics);
		root.Accept(interpreter);

		if (diagnostics.ContainsErrors)
			throw new InvalidOperationException("Interpreting example file produced errors");
	}
}
