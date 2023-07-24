using System.Collections;
using HILFE.Interpreting;
using HILFE.Parsing.Statements;
using HILFE.Tokenizing;

namespace HILFE.Tests.Integration;

public class ExamplesIntegrationTest
{
    [Theory]
    [ClassData(typeof(ExampleFiles))]
    public async Task TestExamplesSucceed(string exampleFilePath)
    {
        var tokenizer = new Tokenizer();
        var parser = new StatementParser();
        var interpreter = new Interpreter(Console.Out, Console.Error, new StringReader("john"));

        var chars = await File.ReadAllTextAsync(exampleFilePath);

        var tokens = tokenizer.TokenizeAsync(chars.ToAsyncEnumerable());

        await parser.AddAsync(tokens);

        var statements = parser.ParseAsync();

        await interpreter.InterpretAsync(statements);

        Assert.Equal(0, interpreter.ExitCode);
    }

    public class ExampleFiles : IEnumerable<object []>
    {
        public IEnumerator<object []> GetEnumerator() => Directory
            .EnumerateFiles("Examples", "*.hil")
            .Select(path => new object[] { path })
            .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}