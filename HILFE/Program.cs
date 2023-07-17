using System.CommandLine;
using HILFE.Interpreting;
using HILFE.Parsing;
using HILFE.Tokenizing;

var configOption = new Option<FileInfo?>(aliases: new []
{
    "-c", "--config",
}, parseArgument: result =>
{
    if (result.Tokens.Count == 0)
    {
        result.ErrorMessage = "Expected file name";
        return null;
    }

    var filePath = result.Tokens.Single().Value;
    if (File.Exists(filePath))
        return new(filePath);

    result.ErrorMessage = $"File not found: {filePath}";
    return null;
}, description: "The configuration file to use.");

var fileArgument = new Argument<FileInfo>(name: "file", description: "The path to a .hil-file")
{
    Arity = ArgumentArity.ExactlyOne,
};

var runCommand = new Command(name: "run", description: "Run a .hil file");
runCommand.AddArgument(fileArgument);

var listenCommand = new Command(name: "listen", description: "Enter REPL mode");

var rootCommand = new RootCommand("HILFE - HILFE Interpreted Language For Education");
rootCommand.AddGlobalOption(configOption);
rootCommand.AddCommand(runCommand);
rootCommand.AddCommand(listenCommand);

listenCommand.SetHandler(async configFile =>
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
    var interpreter = new Interpreter(Console.Out, Console.Error, inputReader);

    Console.WriteLine("Welcome to HILFE REPL! Use Ctrl-C to exit.");
    Console.Write("> ");
    while (Console.ReadLine() is { } line)
    {
        inputWriter.WriteLine(line);

        var chars = line.ToAsyncEnumerable();

        try
        {
            var tokens = await tokenizer.TokenizeAsync(chars).ToListAsync();
            if (tokens.Count == 0)
            {
                Console.Write("| ");

                continue;
            }

            var statements = await parser.ParseAsync(tokens.ToAsyncEnumerable()).ToListAsync();
            if (statements.Count == 0)
            {
                Console.Write("| ");

                continue;
            }

            await interpreter.InterpretAsync(statements.ToAsyncEnumerable());
        }
        catch (ApplicationException e) when (e is ParserException or TokenizerException or InterpreterException)
        {
            await Console.Error.WriteLineAsync("~~> " + e.GetType().Name + ": " + e.Message);
        }

        Console.Write("> ");
    }

    inputWriter.WriteLine("exit 0");
    Console.WriteLine("Bye!");
}, configOption);

runCommand.SetHandler(async (configFile, scriptFile) =>
{
    // var config = configFile != null ? Config.FromFile(configFile) : Config.FromEnvironment();

    var tokenizer = new Tokenizer();
    var parser = new Parser();
    var interpreter = new Interpreter(Console.Out, Console.Error, Console.In);

    var chars = await File.ReadAllTextAsync(scriptFile.FullName);

    try
    {
        var tokens = tokenizer.TokenizeAsync(chars);
        var statements = parser.ParseAsync(tokens);
        await interpreter.InterpretAsync(statements);

        Environment.ExitCode = interpreter.ExitCode;
    }
    catch (ApplicationException e) when (e is ParserException or TokenizerException or InterpreterException)
    {
        await Console.Error.WriteLineAsync("~~> " + e.GetType().Name + ": " + e.Message);

        Environment.ExitCode = -1;
    }
}, configOption, fileArgument);

return await rootCommand.InvokeAsync(args);