using HILFE.Interpreting;
using HILFE.Parsing;
using HILFE.Tokenizing;

namespace HILFE.Tests.Integration
{
    public class ExamplesIntegrationTest
    {
        [Fact]
        public async Task TestExamplesSucceed()
        {
            var tokenizer = new Tokenizer();
            var parser = new Parser();
            var interpreter = new Interpreter(Console.Out, Console.Error, Console.In);

            foreach (var hilFile in System.IO.Directory.EnumerateFiles("Examples", "*.hil"))
            {
                var chars = await File.ReadAllTextAsync(hilFile);

                var tokens = tokenizer.TokenizeAsync(chars.ToAsyncEnumerable());
                await parser.AddAsync(tokens);
                var statements = parser.ParseAsync();
                await interpreter.InterpretAsync(statements);

                Assert.Equal(0, interpreter.ExitCode);
            }
        }
    }
}
