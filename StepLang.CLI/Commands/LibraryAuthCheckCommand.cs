using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;
using StepLang.Libraries.Client;

namespace StepLang.CLI.Commands;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class LibraryAuthCheckCommand : AsyncCommand<HiddenGlobalCommandSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, HiddenGlobalCommandSettings settings)
    {
        using var httpClient = new HttpClient();
        var apiClient = new LibApiClientFactory(null).CreateClient(httpClient);
        var result = await apiClient.CheckAsync();

        if (result?.Code == "success")
        {
            await Console.Out.WriteLineAsync("Successfully authenticated.");

            return 0;
        }

        await Console.Error.WriteLineAsync("Authentication failed: " + result?.Message + " (" + result?.Code + ")");

        return 1;
    }
}