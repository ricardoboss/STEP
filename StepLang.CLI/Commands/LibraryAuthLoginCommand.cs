using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;
using StepLang.Libraries.API;
using StepLang.Libraries.Client;

namespace StepLang.CLI.Commands;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class LibraryAuthLoginCommand : AsyncCommand<LibraryAuthLoginCommand.Settings>
{
    public sealed class Settings : HiddenGlobalCommandSettings
    {
    }

    private readonly LibApiClient apiClient;
    private readonly LibApiCredentialManager credentialManager;

    public LibraryAuthLoginCommand(LibApiClient apiClient, LibApiCredentialManager credentialManager)
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