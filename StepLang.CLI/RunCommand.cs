using System.Diagnostics;
using StepLang.Interpreting;
using StepLang.Statements;
using StepLang.Tokenizing;

namespace StepLang.CLI;

internal static class RunCommand
{
    public static async Task<int> Invoke(FileSystemInfo scriptFile)
    {
        Debug.Assert(scriptFile.Exists);

        var chars = await File.ReadAllTextAsync(scriptFile.FullName);

        var tokenizer = new Tokenizer();
        tokenizer.UpdateFile(scriptFile);
        tokenizer.Add(chars);
        var tokens = tokenizer.Tokenize();

        var parser = new StatementParser();
        await parser.AddAsync(tokens.ToAsyncEnumerable());

        var interpreter = new Interpreter(Console.Out, Console.Error, Console.In);
        var statements = parser.ParseAsync();

        await interpreter.InterpretAsync(statements);

        return interpreter.ExitCode;
    }
}