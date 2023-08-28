using StepLang.Interpreting;
using StepLang.Parsing.Statements;
using StepLang.Tokenizing;

namespace StepLang.Tests.Framework.Pure;

public class SubstringFunctionTest
{
    [Theory]
    [InlineData("substring(\"abc\", 0, 1)", "a")]
    [InlineData("substring(\"Hello, world!\", 0, 5)", "Hello")]
    [InlineData("substring(\"Hello, world!\", 7, 5)", "world")]
    [InlineData("substring(\"Hello, world!\", 7, 100)", "world!")]
    [InlineData("substring(\"Hello, world!\", 7, 0)", "")]
    [InlineData("substring(\"Hello, world!\", 7, -1)", "")]
    [InlineData("substring(\"Hello, world!\", 7, 1.5)", "wo")]
    [InlineData("substring(\"Hello, world!\", -1, 3)", "!")]
    [InlineData("substring(\"Hello, world!\", -2, 3)", "d!")]
    [InlineData("substring(\"Hello, world!\", -3, 3)", "ld!")]
    [InlineData("substring(\"Hello, world!\", -4, 3)", "rld")]
    [InlineData("substring(\"Hello, world!\", -13, 5)", "Hello")]
    public async Task TestSubstring(string code, string result)
    {
        var tokenizer = new Tokenizer();
        tokenizer.Add($"print({code})");
        var tokens = tokenizer.TokenizeAsync();

        var parser = new StatementParser();
        parser.Add(tokens);
        var statements = parser.ParseAsync();

        var output = new StringWriter();
        var interpreter = new Interpreter(output);
        await interpreter.InterpretAsync(statements);

        Assert.Equal(result, output.ToString());
    }
}