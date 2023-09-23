using System.Runtime.InteropServices;
using cmdwtf;
using Spectre.Console;
using Spectre.Console.Cli;
using StepLang.CLI;
using StepLang.CLI.Commands;
using StepLang.Libraries.Client;

const string slogan = "STEP - Simple Transition to Elevated Programming";

await using var services = new ServiceCollectionTypeRegistrar();

services.AddLibApiCredentialManager();
services.AddLibApiClient();

var app = new CommandApp<DefaultCommand>(services)
        .WithDescription(slogan + Environment.NewLine + "Version: " + GitVersionInformation.FullSemVer)
    ;

app.Configure(config =>
{
    config
        .SetApplicationName("step")
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

    config.AddBranch("library", library =>
    {
        library.SetDescription("Manage libraries.");

        library.AddCommand<LibraryInitCommand>("init")
            .WithDescription("Initialize a new library.")
            .WithExample("library init")
            .WithExample("library init --name MyLibrary --author \"John Doe\"")
            ;

        library.AddCommand<LibraryAddCommand>("add")
            .WithDescription("Add a library dependency.")
            .WithExample("library add MyLibrary")
            .WithExample("library add MyLibrary 1.*")
            .WithExample("library add MyLibrary ^1.0.0")
            ;

        library.AddCommand<LibraryPublishCommand>("publish")
            .WithDescription("Publish a library.")
            .WithExample("library publish")
            .WithExample("library publish --version 1.2.3")
            ;

        library.AddBranch("auth", auth =>
        {
            auth.SetDescription("Manage authentication against STEP library.");

            auth.AddCommand<LibraryAuthRegisterCommand>("register")
                .WithDescription("Register a new library account.")
                .WithExample("library auth register")
                ;

            auth.AddCommand<LibraryAuthLoginCommand>("login")
                .WithDescription("Login to a library account.")
                .WithExample("library auth login")
                ;

            auth.AddCommand<LibraryAuthLogoutCommand>("logout")
                .WithDescription("Logout of a library account.")
                .WithExample("library auth logout")
                ;

            auth.AddCommand<LibraryAuthCheckCommand>("check")
                .WithDescription("Check if logged in to a library account.")
                .WithExample("library auth check")
                ;
        });
    });
});

return await app.RunAsync(args);

namespace StepLang.CLI
{
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
}
