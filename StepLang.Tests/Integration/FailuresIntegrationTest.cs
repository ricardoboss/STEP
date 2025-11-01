using StepLang.Diagnostics;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StepLang.Tests.Integration;

public class FailuresIntegrationTest
{
	[TestCaseSource(typeof(FailureFiles))]
	public async Task TestFailuresFail(string exampleFilePath)
	{
		// arrange
		var exampleFile = new FileInfo(exampleFilePath);
		var stdInText = "";
		if (File.Exists(exampleFile.FullName + ".in"))
		{
			stdInText = await File.ReadAllTextAsync(exampleFile.FullName + ".in", TestContext.CurrentContext.CancellationToken);
		}

		var detailsFile = exampleFile.FullName + ".exception.json";
		Assume.That(File.Exists(detailsFile), $"No exception details file found for {exampleFile.FullName}");

		var detailsContent = await File.ReadAllTextAsync(detailsFile, TestContext.CurrentContext.CancellationToken);
		var details =
			JsonSerializer.Deserialize(detailsContent, ExceptionDetailsJsonContext.Default.ListExceptionDetails);

		Assume.That(details, Is.Not.Null, $"Failed to deserialize exception details for {exampleFile.FullName}");
		var parsedDetails = details!;

		await using var stdOut = new StringWriter();
		using var stdIn = new StringReader(stdInText);

		// act
		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(exampleFile, diagnostics);
		var interpreter = new Interpreter(stdOut, stdIn, null, diagnostics);

		// assert
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken);
		if (diagnostics.ContainsErrors)
		{
			AssertErrors(diagnostics, exampleFile, parsedDetails);

			return;
		}

		var parser = new Parser(tokens, diagnostics);
		var root = parser.ParseRoot();
		if (diagnostics.ContainsErrors)
		{
			AssertErrors(diagnostics, exampleFile, parsedDetails);

			return;
		}

		// no tokenizer error and no parser error means the exception must throw at runtime
		var caughtException = Assert.Catch<StepLangException>(() => root.Accept(interpreter));
		Assert.That(caughtException, Is.Not.Null);

		AssertException(caughtException!, exampleFile, parsedDetails);
	}

	[SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
	private static void AssertErrors(DiagnosticCollection diagnostics, FileInfo sourceFile,
		List<ExceptionDetails> details)
	{
		Assert.That(diagnostics, Has.Count.EqualTo(details.Count));

		foreach (var (diagnostic, detail) in diagnostics.Zip(details))
		{
			using (Assert.EnterMultipleScope())
			{
				Assert.That(diagnostic.Severity, Is.EqualTo(detail.Severity));
				Assert.That(diagnostic.Message, Is.EqualTo(detail.Message));
				Assert.That(diagnostic.Code, Is.EqualTo(detail.ErrorCode));
				Assert.That(diagnostic.Kind, Is.EqualTo(detail.Kind));
				Assert.That(diagnostic.Area, Is.EqualTo(detail.Area));
				Assert.That(diagnostic.Line, Is.EqualTo(detail.Line));
				Assert.That(diagnostic.Column, Is.EqualTo(detail.Column));
				Assert.That(diagnostic.Length, Is.EqualTo(detail.Length));
				Assert.That(diagnostic.File?.FullName, Is.EqualTo(sourceFile.FullName));
			}
		}
	}

	private static void AssertException(StepLangException caught, FileInfo sourceFile, List<ExceptionDetails> details)
	{
		if (details.Count != 1)
			Assert.Fail("Expected exactly one exception detail to match against caught exception");

		var detail = details[0];

		using (Assert.EnterMultipleScope())
		{
			Assert.That(caught.ErrorCode, Is.EqualTo(detail.ErrorCode));
			Assert.That(caught.Message, Is.EqualTo(detail.Message));
			Assert.That(caught.HelpText, Is.EqualTo(detail.HelpText));

			if (caught.Location is { } location)
			{
				Assert.That(location.Line, Is.EqualTo(detail.Line));
				Assert.That(location.Column, Is.EqualTo(detail.Column));
				Assert.That(location.Length, Is.EqualTo(detail.Length));
				Assert.That(location.File?.FullName, Is.EqualTo(sourceFile.FullName));
			}
			else
			{
				Assert.That(detail.Line, Is.Null);
				Assert.That(detail.Column, Is.Null);
				Assert.That(detail.Length, Is.Null);
			}
		}
	}

	[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by NUnit")]
	private sealed class FailureFiles : IEnumerable<TestCaseData>
	{
		public IEnumerator<TestCaseData> GetEnumerator()
		{
			return Directory
					.EnumerateFiles("Failures", "*.step")
					.Select(file => new TestCaseData(file))
					.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Local", Justification = "Deserialized from JSON")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local", Justification = "Initialized from JSON")]
internal sealed class ExceptionDetails
{
	public string? ErrorCode { get; init; }
	public string? Message { get; init; }
	public string? HelpText { get; init; }
	[JsonConverter(typeof(JsonStringEnumConverter<Severity>))]
	public Severity? Severity { get; init; }
	public int? Line { get; init; }
	public int? Column { get; init; }
	public int? Length { get; init; }
	[JsonConverter(typeof(JsonStringEnumConverter<DiagnosticKind>))]
	public DiagnosticKind? Kind { get; init; }
	[JsonConverter(typeof(JsonStringEnumConverter<DiagnosticArea>))]
	public DiagnosticArea? Area { get; init; }
}

[JsonSourceGenerationOptions(WriteIndented = true, Converters = [typeof(JsonStringEnumConverter<DiagnosticArea>), typeof(JsonStringEnumConverter<DiagnosticKind>), typeof(JsonStringEnumConverter<Severity>)])]
[JsonSerializable(typeof(List<ExceptionDetails>))]
internal sealed partial class ExceptionDetailsJsonContext : JsonSerializerContext;
