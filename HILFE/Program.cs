using System.CommandLine;
using HILFE;

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

    var interpreter = new Interpreter(config, "<stdin>", inputReader, Console.Out, Console.Error, inputReader);
    var interpreterTask = Task.Run(async () => await interpreter.Interpret());

    Console.WriteLine("Welcome to HILFE REPL! Use Ctrl-C to exit.");
    Console.Write("> ");
    while (Console.ReadLine() is { } line)
    {
        inputWriter.WriteLine(line);
        Console.Write("> ");
    }

    inputWriter.WriteLine("exit 0");
    Console.WriteLine("Bye!");

    Environment.ExitCode = await interpreterTask;
}, configOption);

runCommand.SetHandler(async (configFile, scriptFile) =>
{
    var config = configFile != null ? Config.FromFile(configFile) : Config.FromEnvironment();
    var lineBuffer = new MemoryStream(await File.ReadAllBytesAsync(scriptFile.FullName));
    var inputReader = new StreamReader(lineBuffer);
    var interpreter = new Interpreter(config, scriptFile.FullName, inputReader, Console.Out, Console.Error, Console.In);
    Environment.ExitCode = await interpreter.Interpret();
}, configOption, fileArgument);

return await rootCommand.InvokeAsync(args);