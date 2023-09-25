using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;

namespace StepLang.CLI.Commands;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class LibraryAuthLogoutCommand : AsyncCommand<HiddenGlobalCommandSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, HiddenGlobalCommandSettings settings)
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var credentialsPath = new FileInfo(Path.Combine(appData, "STEP", "credentials.json"));

        if (credentialsPath.Exists)
            credentialsPath.Delete();

        await Console.Out.WriteLineAsync("Credentials deleted.");

        return 0;
    }
}