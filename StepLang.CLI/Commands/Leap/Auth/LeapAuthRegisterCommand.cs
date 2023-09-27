using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Leap.Client;
using Leap.Common.API;
using Spectre.Console.Cli;
using StepLang.CLI.Settings;

namespace StepLang.CLI.Commands.Leap.Auth;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class LeapAuthRegisterCommand : AsyncCommand<LeapAuthRegisterCommand.Settings>
{
    public sealed class Settings : HiddenGlobalCommandSettings
    {
    }

    private readonly LeapApiClient apiClient;
    private readonly LeapApiCredentialManager credentialManager;

    public LeapAuthRegisterCommand(LeapApiClient apiClient, LeapApiCredentialManager credentialManager)
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

        var request = new RegisterRequest(username, password);
        var result = await apiClient.RegisterAsync(request);
        if (result == null)
        {
            await Console.Error.WriteLineAsync("Registration request failed");

            return 1;
        }

        if (result.Code != "success")
        {
            await Console.Error.WriteLineAsync("Registration request failed: " + result.Message + " (" + result.Code + ")");

            return 2;
        }

        Debug.Assert(result.Token is not null);

        credentialManager.StoreCredentials(Credentials.TokenOnly(result.Token), true);

        await Console.Out.WriteLineAsync("Registration successful and credentials saved.");

        return 0;
    }
}