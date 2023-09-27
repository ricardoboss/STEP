using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Leap.Client;
using Leap.Common.API;
using Spectre.Console.Cli;
using StepLang.CLI.Settings;

namespace StepLang.CLI.Commands.Leap.Auth;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class LeapAuthLoginCommand : AsyncCommand<LeapAuthLoginCommand.Settings>
{
    public sealed class Settings : HiddenGlobalCommandSettings
    {
    }

    private readonly LeapApiClient apiClient;
    private readonly LeapApiCredentialManager credentialManager;

    public LeapAuthLoginCommand(LeapApiClient apiClient, LeapApiCredentialManager credentialManager)
    {
        this.apiClient = apiClient;
        this.credentialManager = credentialManager;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        await Console.Out.WriteAsync("Username: ");
        var username = Console.ReadLine() ?? throw new InvalidOperationException("Username must be set");

        await Console.Out.WriteAsync("Password: ");
        var password = Console.ReadLine() ?? throw new InvalidOperationException("Password must be set");

        var request = new CreateTokenRequest(username, password);
        var result = await apiClient.CreateTokenAsync(request);
        if (result == null)
        {
            await Console.Error.WriteLineAsync("Create token request failed");

            return 1;
        }

        if (result.Code != "success")
        {
            await Console.Error.WriteLineAsync("Create token request failed: " + result.Message + " (" + result.Code + ")");

            return 2;
        }

        Debug.Assert(result.Token is not null);

        credentialManager.StoreCredentials(Credentials.TokenOnly(result.Token), true);

        await Console.Out.WriteLineAsync("Credentials saved.");

        return 0;
    }
}