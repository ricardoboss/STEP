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
        var fileArgument = new Argument<FileInfo>(name: "file", description: "The path to a .hil-file")
        {
            Arity = ArgumentArity.ExactlyOne,
        };

        var runCommand = new Command(name: "run", description: "Run a .hil file");
        runCommand.AddArgument(fileArgument);

        var listenCommand = new Command(name: "listen", description: "Enter REPL mode");

        var rootCommand = new RootCommand("STEPHILFE -STEP HILFE Interpreted Language For Education");
        rootCommand.AddCommand(runCommand);
        rootCommand.AddCommand(listenCommand);

        listenCommand.SetHandler(async () =>
        {
            var memoryBuffer = new MemoryStream();
            var inputReader = new StreamReader(memoryBuffer);
            var inputWriter = new StreamWriter(memoryBuffer)
            {
                AutoFlush = true,
            };

            var tokenizer = new Tokenizer();
            var parser = new StatementParser();
            var interpreter = new Interpreter(Console.Out, Console.Error, inputReader, Console.Out);

            Console.WriteLine("Welcome to STEP REPL! Use Ctrl-C to exit.");
            Console.Write("> ");
            while (Console.ReadLine() is { } line)
            {
                await inputWriter.WriteLineAsync(line);

                try
                {
                    var chars = line.Append('\n');

                    tokenizer.Add(chars);
                    var tokens = await tokenizer.TokenizeAsync().ToListAsync();
                    if (tokens.Count == 0)
                    {
                        Console.Write("| ");

                        continue;
                    }

                    parser.Add(tokens);
                    var statements = await parser.ParseAsync().ToListAsync();
                    if (statements.Count == 0)
                    {
                        Console.Write("| ");

                        continue;
                    }

                    await interpreter.InterpretAsync(statements.ToAsyncEnumerable());
                }
                catch (Exception e) when (e is ParserException or TokenizerException or InterpreterException)
                {
                    await Console.Error.WriteLineAsync("~~> " + e.GetType().Name + ": " + e.Message);
                }

                Console.Write("> ");
            }

            await inputWriter.WriteLineAsync("exit 0");
            Console.WriteLine("Bye!");
        });

        runCommand.SetHandler(async scriptFile =>
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

                Environment.ExitCode = interpreter.ExitCode;
            }
            catch (Exception e) when (e is ParserException or TokenizerException or InterpreterException)
            {
                await Console.Error.WriteLineAsync("~~> " + e.GetType().Name + ": " + e.Message);

                Environment.ExitCode = -1;
            }
        }, fileArgument);

        return await rootCommand.InvokeAsync(args);
    }
}