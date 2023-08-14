using System.CommandLine;
using System.Diagnostics.CodeAnalysis;

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

        var fileOrDirArgument = new Argument<string[]>(name: "file-or-dir", description: "The path to a .step-file or a directory of .step-files")
        {
            Arity = ArgumentArity.ZeroOrMore,
        };

        var setExitCodeOption = new Option<bool>(aliases: new[] { "--set-exit-code", "-s" }, description: "Set the exit code to the number of changes made")
        {
            IsRequired = false,
            Arity = ArgumentArity.ZeroOrOne,
        };

        var dryRunOption = new Option<bool>(aliases: new[] { "--dry-run", "-d" }, description: "Don't actually make any changes")
        {
            IsRequired = false,
            Arity = ArgumentArity.ZeroOrOne,
        };

        var verbosityOption = new Option<Verbosity>(aliases: new[] { "--verbosity", "-v" }, description: "Set the verbosity level")
        {
            IsRequired = false,
            Arity = ArgumentArity.ZeroOrOne,
        };

        var formatConfigOption = new Option<FileInfo>(aliases: new[] { "--config", "-c" }, description: "The path to a step-format.json-file")
        {
            IsRequired = false,
            Arity = ArgumentArity.ZeroOrOne,
        };

        var runCommand = new Command(name: "run", description: "Run a .step file");
        runCommand.AddArgument(fileArgument);
        runCommand.SetHandler(RunCommand.Invoke, fileArgument);

        var formatCommand = new Command(name: "format", description: "Format a .step file or a directory of .step files");
        formatCommand.AddArgument(fileOrDirArgument);
        formatCommand.AddOption(setExitCodeOption);
        formatCommand.AddOption(dryRunOption);
        formatCommand.AddOption(verbosityOption);
        formatCommand.AddOption(formatConfigOption);
        formatCommand.SetHandler(FormatCommand.Invoke, fileOrDirArgument, setExitCodeOption, dryRunOption, verbosityOption, formatConfigOption);

        var rootCommand = new RootCommand("STEP - Simple Transition to Elevated Programming");
        rootCommand.AddCommand(runCommand);
        rootCommand.AddCommand(formatCommand);

        return await rootCommand.InvokeAsync(args);
    }
}