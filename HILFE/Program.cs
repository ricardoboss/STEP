using System.CommandLine;
using HILFE.Interpreting;
using HILFE.Parsing;
using HILFE.Tokenizing;

var fileArgument = new Argument<FileInfo>(name: "file", description: "The path to a .hil-file")
{
    Arity = ArgumentArity.ExactlyOne,
};

var runCommand = new Command(name: "run", description: "Run a .hil file");
runCommand.AddArgument(fileArgument);

var listenCommand = new Command(name: "listen", description: "Enter REPL mode");

var rootCommand = new RootCommand("HILFE - HILFE Interpreted Language For Education");
rootCommand.AddCommand(runCommand);
rootCommand.AddCommand(listenCommand);

listenCommand.SetHandler(async () =>
{
    // var config = configFile != null ? Config.FromFile(configFile) : Config.FromEnvironment();
    var memoryBuffer = new MemoryStream();
    var inputReader = new StreamReader(memoryBuffer);
    var inputWriter = new StreamWriter(memoryBuffer)
    {
        AutoFlush = true,
    };

    var tokenizer = new Tokenizer();
    var parser = new Parser();
    var interpreter = new Interpreter(Console.Out, Console.Error, inputReader, Console.Out);

    Console.WriteLine("Welcome to HILFE REPL! Use Ctrl-C to exit.");
    Console.Write("> ");
    while (Console.ReadLine() is { } line)
    {
        await inputWriter.WriteLineAsync(line);

        var chars = line.ToAsyncEnumerable().Append('\n');

        try
        {
            var tokens = await tokenizer.TokenizeAsync(chars).ToListAsync();
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

runCommand.SetHandler(async (scriptFile) =>
{
    var tokenizer = new Tokenizer();
    var parser = new Parser();
    var interpreter = new Interpreter(Console.Out, Console.Error, Console.In);

    var chars = await File.ReadAllTextAsync(scriptFile.FullName);

    try
    {
        var tokens = tokenizer.TokenizeAsync(chars.ToAsyncEnumerable());
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