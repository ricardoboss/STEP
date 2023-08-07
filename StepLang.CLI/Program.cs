using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Pastel;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Parsing.Statements;
using StepLang.Tokenizing;

namespace StepLang.CLI;

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
        if (!scriptFile.Exists)
        {
            await Console.Error.WriteLineAsync(FormatError("File Not Found", $"The file '{scriptFile.FullName.Pastel(Color.Aqua)}' does not exist."));

            return -1;
        }

        try
        {
            var chars = await File.ReadAllTextAsync(scriptFile.FullName);

            var tokenizer = new Tokenizer
            {
                File = scriptFile,
            };

            tokenizer.Add(chars);
            var tokens = tokenizer.TokenizeAsync();

            var parser = new StatementParser();
            await parser.AddAsync(tokens.ToAsyncEnumerable());

            var interpreter = new Interpreter(Console.Out, Console.Error, Console.In);
            var statements = parser.ParseAsync();

            await interpreter.InterpretAsync(statements);

            return interpreter.ExitCode;
        }
        catch (Exception e) when (e is ParserException or TokenizerException or InterpreterException or IOException)
        {
            await Console.Error.WriteLineAsync(FormatError(e));

            return -1;
        }
    }

    private static string FormatError(string type, string message) => ("! " + type + ": ").Pastel(Color.OrangeRed) + message;

    private static string FormatError(Exception e) => FormatError(e.GetType().Name, e.Message + Environment.NewLine + e.StackTrace.Pastel(Color.DarkGray));
}