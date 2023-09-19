using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using cmdwtf;

namespace StepLang.CLI;

[ExcludeFromCodeCoverage]
internal static class Program
{
    private const string Slogan = "STEP - Simple Transition to Elevated Programming";

    public static async Task<int> Main(string[] args)
    {
        var fileArgument = new Argument<FileInfo>(name: "file", description: "The path to a .step-file")
        {
            Arity = ArgumentArity.ExactlyOne,
        }.LegalFilePathsOnly().ExistingOnly().WithStepExtensionOnly();

        var hiddenFileArgument = new Argument<FileInfo>(name: "file", description: "The path to a .step-file")
        {
            Arity = ArgumentArity.ExactlyOne,
            IsHidden = true,
        }.LegalFilePathsOnly().ExistingOnly().WithStepExtensionOnly();

        var fileOrDirArgument = new Argument<string[]>(name: "file-or-dir", description: "The path to a .step-file or a directory of .step-files")
        {
            Arity = ArgumentArity.ZeroOrMore,
        }.LegalFilePathsOnly();

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

        var verbosityOption = new Option<Verbosity>(aliases: new[] { "--verbosity", "-v" }, description: "Set the output verbosity level", getDefaultValue: () => Verbosity.Normal)
        {
            IsRequired = false,
            Arity = ArgumentArity.ZeroOrOne,
        };

        var versionOption = new Option<bool>(aliases: new[] { "--version" }, description: "Print the version number")
        {
            IsRequired = false,
            Arity = ArgumentArity.ZeroOrOne,
        };

        var infoOption = new Option<bool>(aliases: new[] { "--info" }, description: "Print the version number and other information")
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
        formatCommand.SetHandler(FormatCommand.Invoke, fileOrDirArgument, setExitCodeOption, dryRunOption, verbosityOption);

        var rootCommand = new RootCommand(Slogan);
        rootCommand.AddArgument(hiddenFileArgument);
        rootCommand.AddOption(versionOption);
        rootCommand.AddOption(infoOption);
        rootCommand.SetHandler(RunCommand.Invoke, hiddenFileArgument);

        rootCommand.AddCommand(runCommand);
        rootCommand.AddCommand(formatCommand);

        var builder = new CommandLineBuilder(rootCommand)
            .UseHelp()
            .UseEnvironmentVariableDirective()
            .UseSuggestDirective()
            .RegisterWithDotnetSuggest()
            .UseTypoCorrections()
            .UseParseDirective(-3)
            .UseParseErrorReporting(-2)
            .UseExceptionHandler(ErrorHandler.HandleException)
            .CancelOnProcessTermination();

        builder.AddMiddleware(async (context, next) =>
        {
            if (context.ParseResult.FindResultFor(versionOption) is not null)
            {
                HandleVersionOption(context);

                return;
            }

            if (context.ParseResult.FindResultFor(infoOption) is not null)
            {
                HandleInfoOption(context);

                return;
            }

            await next(context);
        }, MiddlewareOrder.Configuration);

        return await builder.Build().InvokeAsync(args);
    }

    private static void HandleVersionOption(InvocationContext context)
    {
        context.Console.Out.WriteLine(GitVersionInformation.FullSemVer);
    }

    private static void HandleInfoOption(InvocationContext context)
    {
        context.Console.Out.WriteLine($"""
                                   {Slogan}
                                   Build Date:  {BuildTimestamp.BuildTimeUtc:yyyy-MM-dd HH:mm:ss} UTC
                                   Version:     {GitVersionInformation.Sha} ({GitVersionInformation.MajorMinorPatch}+{GitVersionInformation.CommitsSinceVersionSource})
                                   Branch:      {GitVersionInformation.BranchName}
                                   CLR Version: {Environment.Version}
                                   OS Version:  {RuntimeInformation.OSDescription} ({RuntimeInformation.OSArchitecture})
                                   """);
    }
}