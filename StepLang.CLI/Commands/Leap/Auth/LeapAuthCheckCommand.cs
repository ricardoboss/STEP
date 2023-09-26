using System.Diagnostics.CodeAnalysis;
using Leap.Client;
using Spectre.Console.Cli;

namespace StepLang.CLI.Commands.Leap.Auth;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class LeapAuthCheckCommand : AsyncCommand<HiddenGlobalCommandSettings>
{
    private readonly LeapApiClient apiClient;

    public LeapAuthCheckCommand(LeapApiClient apiClient)
    {
        this.apiClient = apiClient;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, HiddenGlobalCommandSettings settings)
    {
        var result = await apiClient.CheckAsync();
        if (result is null)
        {
            await Console.Out.WriteLineAsync("Request failed.");

            return 1;
        }

        if (result.Code == "success")
        {
            await Console.Out.WriteLineAsync($"Authentication successful: {result.Message}");

            return 0;
        }

        await Console.Error.WriteLineAsync($"Authentication failed: {result.Message} ({result.Code})");

        return 1;
    }
}