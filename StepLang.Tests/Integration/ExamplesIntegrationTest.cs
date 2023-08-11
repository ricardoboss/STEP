using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using StepLang.Interpreting;
using StepLang.Parsing.Statements;
using StepLang.Tokenizing;

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
            stdInText = await File.ReadAllTextAsync(exampleFile.FullName + ".in");

        var stdOut = new StringWriter();
        var stdErr = new StringWriter();
        var stdIn = new StringReader(stdInText);

        var expectedExitCode = 0;
        var expectedOutput = "";
        var expectedError = "";

        if (File.Exists(exampleFile.FullName + ".exit"))
            expectedExitCode = int.Parse(await File.ReadAllTextAsync(exampleFile.FullName + ".exit"), CultureInfo.InvariantCulture);
        if (File.Exists(exampleFile.FullName + ".out"))
            expectedOutput = await File.ReadAllTextAsync(exampleFile.FullName + ".out");
        if (File.Exists(exampleFile.FullName + ".err"))
            expectedError = await File.ReadAllTextAsync(exampleFile.FullName + ".err");

        var tokenizer = new Tokenizer();
        tokenizer.UpdateFile(exampleFile);
        var parser = new StatementParser();
        var interpreter = new Interpreter(stdOut, stdErr, stdIn);

        // act
        var chars = await File.ReadAllTextAsync(exampleFile.FullName);
        tokenizer.Add(chars);
        var tokens = tokenizer.Tokenize();
        await parser.AddAsync(tokens.ToAsyncEnumerable());
        var statements = parser.ParseAsync();
        await interpreter.InterpretAsync(statements);

        // assert
        Assert.Equal(expectedExitCode, interpreter.ExitCode);
        Assert.Equal(expectedOutput, stdOut.ToString(), ignoreLineEndingDifferences: true);
        Assert.Equal(expectedError, stdErr.ToString(), ignoreLineEndingDifferences: true);
    }

    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Used by xUnit")]
    private sealed class ExampleFiles : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator() => Directory
            .EnumerateFiles("Examples", "*.step")
            .Select(path => new object[] { new FileInfo(path) })
            .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}