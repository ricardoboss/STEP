using System.CommandLine;
using HILFE;
using HILFE.Interpreting;

var configOption = new Option<FileInfo?>(aliases: new[] { "-c", "--config" }, parseArgument: result =>
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
    var config = configFile != null ? Config.FromFile(configFile) : Config.FromEnvironment();
    var memoryBuffer = new MemoryStream();
    var inputReader = new StreamReader(memoryBuffer);
    var inputWriter = new StreamWriter(memoryBuffer)
    {
        AutoFlush = true,
    };

    var interpreter = new Interpreter(config, "<stdin>", Console.Out, Console.Error, inputReader);

    Console.WriteLine("Welcome to HILFE REPL! Use Ctrl-C to exit.");
    Console.Write("> ");
    while (Console.ReadLine() is { } line)
    {
        inputWriter.WriteLine(line);

        await interpreter.InterpretAsync(line.ToAsyncEnumerable().Concat(new [] { '\n' }.ToAsyncEnumerable()));

        Console.Write("> ");
    }

    inputWriter.WriteLine("exit 0");
    Console.WriteLine("Bye!");
}, configOption);

runCommand.SetHandler(async (configFile, scriptFile) =>
{
    var config = configFile != null ? Config.FromFile(configFile) : Config.FromEnvironment();
    var chars = await File.ReadAllTextAsync(scriptFile.FullName);
    var interpreter = new Interpreter(config, scriptFile.FullName, Console.Out, Console.Error, Console.In);
    Environment.ExitCode = await interpreter.InterpretAsync(chars.ToAsyncEnumerable());
}, configOption, fileArgument);

return await rootCommand.InvokeAsync(args);