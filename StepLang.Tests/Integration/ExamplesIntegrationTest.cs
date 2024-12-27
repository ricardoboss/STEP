using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace StepLang.Tests.Integration;

public class ExamplesIntegrationTest
{
	[Theory]
	[ClassData(typeof(ExampleFiles))]
	public async Task TestExamplesSucceed(FileInfo exampleFile)
	{
		// arrange
		var stdInText = "";
		if (File.Exists(exampleFile.FullName + ".in"))
		{
			stdInText = await File.ReadAllTextAsync(exampleFile.FullName + ".in");
		}

		await using var stdOut = new StringWriter();
		await using var stdErr = new StringWriter();
		using var stdIn = new StringReader(stdInText);

		var expectedExitCode = 0;
		var expectedOutput = "";
		var expectedError = "";

		if (File.Exists(exampleFile.FullName + ".exit"))
		{
			expectedExitCode = int.Parse(await File.ReadAllTextAsync(exampleFile.FullName + ".exit"),
				CultureInfo.InvariantCulture);
		}

		if (File.Exists(exampleFile.FullName + ".out"))
		{
			expectedOutput = await File.ReadAllTextAsync(exampleFile.FullName + ".out");
		}

		if (File.Exists(exampleFile.FullName + ".err"))
		{
			expectedError = await File.ReadAllTextAsync(exampleFile.FullName + ".err");
		}


		// act
		var tokenizer = new Tokenizer(exampleFile);
		var tokens = tokenizer.Tokenize();
		var parser = new Parser(tokens);
		var root = parser.ParseRoot();
		var interpreter = new Interpreter(stdOut, stdErr, stdIn);
		root.Accept(interpreter);

		// assert
		Assert.Equal(expectedExitCode, interpreter.ExitCode);
		Assert.Equal(expectedOutput, stdOut.ToString(), ignoreLineEndingDifferences: true);
		Assert.Equal(expectedError, stdErr.ToString(), ignoreLineEndingDifferences: true);
	}

	[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Used by xUnit")]
	private sealed class ExampleFiles : IEnumerable<object[]>
	{
		public IEnumerator<object[]> GetEnumerator()
		{
			return Directory
				.EnumerateFiles("Examples", "*.step", SearchOption.AllDirectories)
				.Select(path => new object[] { new FileInfo(path) })
				.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
