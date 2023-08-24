using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using Pastel;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Parsing.Statements;
using StepLang.Tokenizing;

namespace StepLang.CLI;

[ExcludeFromCodeCoverage]
internal static class Program
{
    public static async Task<int> Main(string[] args)
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

            var tokenizer = new Tokenizer();
            tokenizer.UpdateFile(scriptFile);
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

    private static string FormatError(Exception e)
    {
        return e switch
        {
            StepLangException sle => FormatStepLangException(sle),
            _ => FormatGeneralException(e),
        };
    }

    private static string FormatGeneralException(Exception e) => FormatError(e.GetType().Name,
        e.Message + Environment.NewLine + e.StackTrace.Pastel(Color.DarkGray));

    private static string FormatStepLangException(StepLangException e)
    {
        const int contextLineCount = 4;

        IEnumerable<string> outputLines;
        var exceptionName = (" " + e.GetType().Name + " ").Pastel(ConsoleColor.White).PastelBg(ConsoleColor.Red);
        var message = Environment.NewLine + "\t" + e.Message + Environment.NewLine;

        if (e.Location is { } location)
        {
            var sourceCode = location.File.Exists ? File.ReadAllText(location.File.FullName) : "";
            var lines = sourceCode.ReplaceLineEndings().Split(Environment.NewLine);
            var contextStartLine = Math.Max(0, location.Line - 1 - contextLineCount);
            var contextEndLine = Math.Min(lines.Length, location.Line + contextLineCount);
            var lineNumber = contextStartLine;
            var lineNumberWidth = contextEndLine.ToString(CultureInfo.InvariantCulture).Length;
            var contextLines = lines[contextStartLine..contextEndLine].Select(l =>
            {
                var prefix = lineNumber == location.Line - 1 ? $"{">".Pastel(ConsoleColor.Red)} " : "  ";

                var displayLineNumber = (lineNumber + 1).ToString(CultureInfo.InvariantCulture);
                var line = prefix + $"{(displayLineNumber.PadLeft(lineNumberWidth) + "|").Pastel(ConsoleColor.Gray)} {l}";

                lineNumber++;

                return line;
            });

            var locationString = $"at {location.File.FullName.Pastel(ConsoleColor.Green)}:{location.Line}";

            outputLines = contextLines.Prepend(locationString);
        }
        else
            outputLines = new[] { (e.StackTrace ?? string.Empty).Pastel(ConsoleColor.Gray) };

        outputLines = outputLines.Prepend(message).Prepend(exceptionName);

        if (e.HelpText is { } helpText)
            outputLines = outputLines.Append(Environment.NewLine + "Tip: ".Pastel(ConsoleColor.DarkCyan) + helpText);

        return Environment.NewLine + string.Join(Environment.NewLine, outputLines);
    }
}