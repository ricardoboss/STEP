using BenchmarkDotNet.Attributes;
using StepLang.Diagnostics;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Parsing.Nodes;
using StepLang.Tokenizing;

namespace StepLang.Benchmarks;

public class ExampleFileBenchmark
{
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

	[ParamsSource(nameof(ExampleFileNames))]
	public string ExampleFileName { get; set; } = null!;

	private CharacterSource Source { get; set; } = null!;

	private IReadOnlyList<Token> Tokens { get; set; } = null!;

	private RootNode Root { get; set; } = null!;

	[GlobalSetup]
	public void PrepareSource()
	{
		var exampleFile = new FileInfo(Path.Combine("Examples", ExampleFileName));
		if (!exampleFile.Exists)
			throw new InvalidOperationException("Example file does not exist: " + exampleFile.FullName);

		// exclude IO operations from benchmarks by loading the file now
		Source = File.ReadAllText(exampleFile.FullName);
	}

	[GlobalSetup(Targets = [nameof(Parse), nameof(Interpret)])]
	public void PrepareTokens()
	{
		Tokens = Tokenize();
	}

	[GlobalSetup(Targets = [nameof(Interpret)])]
	public void PrepareAst()
	{
		Root = Parse();
	}

	[Benchmark]
	public IReadOnlyList<Token> Tokenize()
	{
		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(Source, diagnostics);
		var tokens = tokenizer.Tokenize().ToList();

		if (diagnostics.ContainsErrors)
			throw new InvalidOperationException("Example file contains tokenizer errors");

		return tokens;
	}

	[Benchmark]
	public RootNode Parse()
	{
		var diagnostics = new DiagnosticCollection();

		var parser = new Parser(Tokens, diagnostics);
		var root = parser.ParseRoot();

		if (diagnostics.ContainsErrors)
			throw new InvalidOperationException("Example file contains parser errors");

		return root;
	}

	[Benchmark]
	public void Interpret()
	{
		var diagnostics = new DiagnosticCollection();

		var interpreter = new Interpreter(null, null, null, diagnostics);
		Root.Accept(interpreter);

		if (diagnostics.ContainsErrors)
			throw new InvalidOperationException("Interpreting example file produced errors");
	}
}
