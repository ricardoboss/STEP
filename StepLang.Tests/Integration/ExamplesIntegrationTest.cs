using StepLang.Diagnostics;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace StepLang.Tests.Integration;

public class ExamplesIntegrationTest
{
	[TestCaseSource(typeof(ExampleFiles))]
	public async Task TestExamplesSucceed(string exampleFilePath)
	{
		// arrange
		var exampleFile = new FileInfo(exampleFilePath);
		var stdInText = "";
		if (File.Exists(exampleFile.FullName + ".in"))
		{
			stdInText = await File.ReadAllTextAsync(exampleFile.FullName + ".in",
				TestContext.CurrentContext.CancellationToken);
		}

		await using var stdOut = new StringWriter();
		await using var stdErr = new StringWriter();
		using var stdIn = new StringReader(stdInText);

		var expectedExitCode = 0;
		var expectedOutput = "";
		var expectedError = "";

		if (File.Exists(exampleFile.FullName + ".exit"))
		{
			var exitCodeStr =
				await File.ReadAllTextAsync(exampleFile.FullName + ".exit", TestContext.CurrentContext.CancellationToken);

			expectedExitCode = int.Parse(exitCodeStr, CultureInfo.InvariantCulture);
		}

		if (File.Exists(exampleFile.FullName + ".out"))
		{
			expectedOutput =
				await File.ReadAllTextAsync(exampleFile.FullName + ".out", TestContext.CurrentContext.CancellationToken);
		}

		if (File.Exists(exampleFile.FullName + ".err"))
		{
			expectedError =
				await File.ReadAllTextAsync(exampleFile.FullName + ".err", TestContext.CurrentContext.CancellationToken);
		}

		// act
		var diagnostics = new DiagnosticCollection();

		var tokenizer = new Tokenizer(exampleFile, diagnostics);
		var tokens = tokenizer.Tokenize(TestContext.CurrentContext.CancellationToken);

		var parser = new Parser(tokens, diagnostics);
		var root = parser.ParseRoot();

		var interpreter = new Interpreter(stdOut, stdErr, stdIn, null, diagnostics);
		root.Accept(interpreter);

		// assert
		Assert.Multiple([SuppressMessage("ReSharper", "AccessToDisposedClosure")] () =>
		{
			Assert.That(interpreter.ExitCode, Is.EqualTo(expectedExitCode));
			Assert.That(NormalizeNewLines(stdOut.ToString()), Is.EqualTo(NormalizeNewLines(expectedOutput)));
			Assert.That(NormalizeNewLines(stdErr.ToString()), Is.EqualTo(NormalizeNewLines(expectedError)));
			Assert.That(diagnostics, Is.Empty);
		});
	}

	private static string NormalizeNewLines(string value)
	{
		return value.Replace("\r\n", "\n", StringComparison.Ordinal);
	}

	[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by NUnit")]
	private sealed class ExampleFiles : IEnumerable<TestCaseData>
	{
		public IEnumerator<TestCaseData> GetEnumerator()
		{
			return Directory
					.EnumerateFiles("Examples", "*.step", SearchOption.AllDirectories)
					.Select(path => new TestCaseData(path))
					.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
