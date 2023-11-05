using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Tests.Integration;

public class FailuresIntegrationTest
{
    [SkippableTheory]
    [ClassData(typeof(FailureFiles))]
    public async Task TestFailuresFail(FileInfo exampleFile)
    {
        // arrange
        var stdInText = "";
        if (File.Exists(exampleFile.FullName + ".in"))
            stdInText = await File.ReadAllTextAsync(exampleFile.FullName + ".in");

        var detailsFile = exampleFile.FullName + ".exception.json";
        Skip.IfNot(File.Exists(detailsFile), $"No exception details file found for {exampleFile.FullName}");

        var details = JsonSerializer.Deserialize<ExceptionDetails?>(await File.ReadAllTextAsync(detailsFile));
        Skip.If(details is null, $"Failed to deserialize exception details for {exampleFile.FullName}");

        var stdOut = new StringWriter();
        var stdErr = new StringWriter();
        var stdIn = new StringReader(stdInText);

        var tokenizer = new Tokenizer();
        tokenizer.UpdateFile(exampleFile);
        var interpreter = new Interpreter(stdOut, stdErr, stdIn);

        // act
        var chars = await File.ReadAllTextAsync(exampleFile.FullName);
        tokenizer.Add(chars);

        // assert
        var exception = Assert.ThrowsAny<StepLangException>(() =>
        {
            var tokens = tokenizer.Tokenize();
            var parser = new Parser(tokens);
            var root = parser.ParseRoot();
            interpreter.Run(root);
        });

        Assert.Equal(details.Message, exception.Message);
        Assert.Equal(details.HelpText, exception.HelpText);
        Assert.Equal(exampleFile, exception.Location?.File);
        Assert.Equal(details.Line, exception.Location?.Line);
        Assert.Equal(details.Column, exception.Location?.Column);
    }

    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Used by xUnit")]
    private sealed class FailureFiles : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator() => Directory
            .EnumerateFiles("Failures", "*.step")
            .Select(path => new object[] { new FileInfo(path) })
            .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local", Justification = "Deserialized from JSON")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local", Justification = "Initialized from JSON")]
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Used by xUnit")]
    private sealed class ExceptionDetails
    {
        public string? Message { get; init; }
        public string? HelpText { get; init; }
        public int Line { get; init; }
        public int Column { get; init; }
    }
}