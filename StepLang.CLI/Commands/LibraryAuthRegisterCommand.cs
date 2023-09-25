using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;
using Leap.Common.API;
using Leap.Client;

namespace StepLang.CLI.Commands;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class LibraryAuthRegisterCommand : AsyncCommand<LibraryAuthRegisterCommand.Settings>
{
    public sealed class Settings : HiddenGlobalCommandSettings
    {
    }

    private readonly LibApiClient apiClient;
    private readonly LibApiCredentialManager credentialManager;

    public LibraryAuthRegisterCommand(LibApiClient apiClient, LibApiCredentialManager credentialManager)
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