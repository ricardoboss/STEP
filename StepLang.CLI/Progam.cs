using System.Runtime.InteropServices;
using cmdwtf;
using Spectre.Console;
using Spectre.Console.Cli;
using StepLang.CLI;
using StepLang.CLI.Commands;

const string slogan = "STEP - Simple Transition to Elevated Programming";

var app = new CommandApp<DefaultCommand>()
        .WithDescription(slogan + Environment.NewLine + "Version: " + GitVersionInformation.FullSemVer)
    ;

app.Configure(config =>
{
    config
        .SetApplicationName("step")
        .SetApplicationVersion(GitVersionInformation.FullSemVer)
        .CaseSensitivity(CaseSensitivity.None)
        .UseStrictParsing()
#if DEBUG
        .ValidateExamples()
#endif
        .SetExceptionHandler(ErrorHandler.HandleException)
        ;

    config.SetInterceptor(new OptionInterceptor());

    config.AddExample("script.step");
    config.AddExample("format script.step");
    config.AddExample("--version");

    config.AddCommand<RunCommand>("run")
        .WithDescription("Run a .step-file.")
        .WithExample("run my-script.step")
        ;

    config.AddCommand<FormatCommand>("format")
        .WithDescription("Format one or more .step-files.")
        .WithExample("format my-script.step")
        .WithExample("format . --dry-run")
        .WithExample("format foo/bar main.step --set-exit-code")
        ;

    config.AddCommand<HighlightCommand>("highlight")
        .WithDescription("Highlight a .step-file and print it to the console.")
        .WithExample("highlight my-script.step")
        .WithExample("highlight my-script.step -t mono")
        .WithExample("highlight --list-themes")
        ;
});

return await app.RunAsync(args);

internal sealed class OptionInterceptor : ICommandInterceptor
{
    public void Intercept(CommandContext context, CommandSettings settings)
    {
        if (settings is not IGlobalCommandSettings globalSettings)
            return;

        if (globalSettings.Version)
        {
            HandleVersionOption();
        }
        else if (globalSettings.Info)
        {
            HandleInfoOption();
        }
    }

    private static void HandleVersionOption() => AnsiConsole.WriteLine(GitVersionInformation.FullSemVer);

    private static void HandleInfoOption()
    {
        var data = new Dictionary<string, string>
        {
            { "Build Date", $"{BuildTimestamp.BuildTimeUtc:yyyy-MM-dd HH:mm:ss} UTC" },
            { "Version", $"{GitVersionInformation.Sha} ({GitVersionInformation.FullSemVer})" },
            { "Branch", GitVersionInformation.BranchName },
            { "CLR Version", Environment.Version.ToString() },
            { "OS Version", $"{RuntimeInformation.OSDescription} ({RuntimeInformation.OSArchitecture})" },
        };

        var infoGrid = new Grid().AddColumns(2);

        var headerStyle = new Style(decoration: Decoration.Bold);

        foreach (var (name, value) in data)
            infoGrid.AddRow(new Text(name, headerStyle).RightJustified(), new Text(value));

        AnsiConsole.Write(infoGrid);
    }
}
