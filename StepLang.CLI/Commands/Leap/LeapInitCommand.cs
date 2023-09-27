using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Leap.Common;
using Spectre.Console;
using Spectre.Console.Cli;
using StepLang.CLI.Extensions;
using StepLang.CLI.Settings;

namespace StepLang.CLI.Commands.Leap;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class LeapInitCommand : AsyncCommand<LeapInitCommand.Settings>
{
    public sealed class Settings : HiddenGlobalCommandSettings
    {
        [CommandOption("-n|--name <name>")]
        [Description("The name of the library to create.")]
        public string? Name { get; init; }

        [CommandOption("-a|--author <author>")]
        [Description("The author of the library to create.")]
        public string? Author { get; init; }
    }

    public override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        if (Library.IsCurrentDirLibrary())
        {
            AnsiConsole.MarkupLine("[red]Error: Current directory is already a library.[/]");

            return Task.FromResult(1);
        }

        var builder = new LibraryBuilder();

        string name;
        do
        {
            name = settings.Name ?? AnsiConsole.Ask<string>("Name of the library: ");

            try
            {
                Library.ValidateName(name);

                break;
            }
            catch (ValidationException ve)
            {
                AnsiConsole.MarkupLineInterpolated($"[red]Invalid library name: {ve.Message}[/]");
            }
        } while (true);

        var author = settings.Author ?? AnsiConsole.Ask("Your name:", Library.GetAuthorPart(name));

        builder.WithName(name);
        if (!string.IsNullOrWhiteSpace(author))
            builder.WithAuthor(author);

        // MAYBE: ask the user if they want to add a description

        // add some defaults
        builder.WithVersion(new(1, 0, 0));

        // MAYBE: ask the user if they want all files in the current dir to be included in the library
        builder.WithFiles(new List<string>());

        var library = builder.Build().SaveToCurrentDir();

        AnsiConsole.MarkupLine($"Created library '{library.MarkupName()}'");

        return Task.FromResult(0);
    }
}