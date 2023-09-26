using System.Runtime.InteropServices;
using cmdwtf;
using Leap.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;
using StepLang.CLI;
using StepLang.CLI.Commands;
using StepLang.CLI.Commands.Leap;
using StepLang.CLI.Commands.Leap.Auth;

const string slogan = "STEP - Simple Transition to Elevated Programming";

await using var services = new ServiceCollectionTypeRegistrar();

var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
using var configurationManager = new ConfigurationManager();
var configuration = configurationManager
    .AddJsonFile("appsettings.json", true)
    .AddJsonFile($"appsettings.{environmentName}.json", true)
    .AddJsonFile("appsettings.local.json", true)
    .Build()
    ;
services.AddSingleton<IConfiguration>(configuration);

services.AddLeapCredentialManager();
services.AddLeapClient();

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

    config.AddBranch("leap", leap =>
    {
        leap.SetDescription("Manage libraries for LEAP (Library ExchAnge Platform).");

        leap.AddCommand<LeapInitCommand>("init")
            .WithDescription("Initialize a new library.")
            .WithExample("leap init")
            .WithExample("leap init --name MyLibrary --author \"John Doe\"")
            ;

        leap.AddCommand<LeapAddCommand>("add")
            .WithDescription("Add a library dependency.")
            .WithExample("leap add MyLibrary")
            .WithExample("leap add MyLibrary 1.*")
            .WithExample("leap add MyLibrary ^1.0.0")
            ;

        leap.AddCommand<LeapPublishCommand>("publish")
            .WithDescription("Publish a library. (requires authentication)")
            .WithExample("leap publish")
            .WithExample("leap publish --version 1.2.3")
            ;

        leap.AddCommand<LeapInstallCommand>("install")
            .WithDescription("Install all dependencies")
            .WithExample("leap install")
            ;

        leap.AddBranch("auth", auth =>
        {
            auth.SetDescription("Manage authentication for LEAP (Library ExchAnge Platform).");

            auth.AddCommand<LeapAuthRegisterCommand>("register")
                .WithDescription("Register a new LEAP account.")
                .WithExample("leap auth register")
                ;

            auth.AddCommand<LeapAuthLoginCommand>("login")
                .WithDescription("Login to a LEAP account.")
                .WithExample("leap auth login")
                ;

            auth.AddCommand<LeapAuthLogoutCommand>("logout")
                .WithDescription("Logout of a LEAP account.")
                .WithExample("leap auth logout")
                ;

            auth.AddCommand<LeapAuthCheckCommand>("check")
                .WithDescription("Check if logged in to a LEAP account.")
                .WithExample("leap auth check")
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
