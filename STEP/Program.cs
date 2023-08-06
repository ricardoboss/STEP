using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using STEP.Interpreting;
using STEP.Parsing;
using STEP.Parsing.Statements;
using STEP.Tokenizing;

namespace STEP;

[ExcludeFromCodeCoverage]
internal static class Program
{
    public static async Task<int> Main(string [] args)
    {
        var fileArgument = new Argument<FileInfo>(name: "file", description: "The path to a .step-file")
        {
            Arity = ArgumentArity.ExactlyOne,
        };

        var runCommand = new Command(name: "run", description: "Run a .step file");
        runCommand.AddArgument(fileArgument);
        runCommand.SetHandler(Run, fileArgument);

        var rootCommand = new RootCommand("STEP - Simple Transition to Elevated Programming");
        rootCommand.AddCommand(runCommand);

        return await rootCommand.InvokeAsync(args);
    }

    private static async Task<int> Run(FileSystemInfo scriptFile)
    {
        var tokenizer = new Tokenizer();
        var parser = new StatementParser();
        var interpreter = new Interpreter(Console.Out, Console.Error, Console.In);

        var chars = await File.ReadAllTextAsync(scriptFile.FullName);

        try
        {
            tokenizer.Add(chars);
            var tokens = tokenizer.TokenizeAsync();
            await parser.AddAsync(tokens);
            var statements = parser.ParseAsync();
            await interpreter.InterpretAsync(statements);

            return interpreter.ExitCode;
        }
        catch (Exception e) when (e is ParserException or TokenizerException or InterpreterException)
        {
            await Console.Error.WriteLineAsync("~~> " + e.GetType().Name + ": " + e.Message);

            return -1;
        }
    }
}