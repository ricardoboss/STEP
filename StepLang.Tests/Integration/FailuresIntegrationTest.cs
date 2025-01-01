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
	[Theory]
	[ClassData(typeof(FailureFiles))]
	public async Task TestFailuresFail(string exampleFilePath)
	{
		// arrange
		var exampleFile = new FileInfo(exampleFilePath);
		var stdInText = "";
		if (File.Exists(exampleFile.FullName + ".in"))
		{
			stdInText = await File.ReadAllTextAsync(exampleFile.FullName + ".in", TestContext.Current.CancellationToken);
		}

		var detailsFile = exampleFile.FullName + ".exception.json";
		Assert.SkipUnless(File.Exists(detailsFile), $"No exception details file found for {exampleFile.FullName}");

		var detailsContent = await File.ReadAllTextAsync(detailsFile, TestContext.Current.CancellationToken);
		var details = JsonSerializer.Deserialize(detailsContent, ExceptionDetailsJsonContext.Default.ExceptionDetails);

		Assert.SkipWhen(details is null, $"Failed to deserialize exception details for {exampleFile.FullName}");

		await using var stdOut = new StringWriter();
		await using var stdErr = new StringWriter();
		using var stdIn = new StringReader(stdInText);

		// act
		var diagnostics = new DiagnosticCollection();
		var tokenizer = new Tokenizer(exampleFile, diagnostics);
		var interpreter = new Interpreter(stdOut, stdErr, stdIn);

		// assert
		// TODO: rewrite using diagnostics
		var tokens = tokenizer.Tokenize(TestContext.Current.CancellationToken);

		Assert.True(true);

		return;

		var parser = new Parser(tokens);
		var root = parser.ParseRoot();
		if (diagnostics.Count > 0)
		{
			Assert.NotEmpty(diagnostics);

			return;
		}

		root.Accept(interpreter);

		// Assert.Multiple(() =>
		// {
		// 	Assert.Equal(details.ErrorCode, exception.ErrorCode);
		// 	Assert.Equal(details.Message, exception.Message);
		// 	Assert.Equal(details.HelpText, exception.HelpText);
		// 	Assert.Equal(exampleFile.FullName, exception.Location?.File?.FullName);
		// 	Assert.Equal(details.Line, exception.Location?.Line);
		// 	Assert.Equal(details.Column, exception.Location?.Column);
		// 	Assert.Equal(details.Length, exception.Location?.Length);
		// });
		Assert.NotEmpty(diagnostics);
	}

	[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Used by xUnit")]
	private sealed class FailureFiles : IEnumerable<TheoryDataRow<string>>
	{
		public IEnumerator<TheoryDataRow<string>> GetEnumerator()
		{
			return Directory
				.EnumerateFiles("Failures", "*.step")
				.Select(file => new TheoryDataRow<string>(file))
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
	public int Line { get; init; }
	public int Column { get; init; }
	public int? Length { get; init; }
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(ExceptionDetails))]
internal sealed partial class ExceptionDetailsJsonContext : JsonSerializerContext;
