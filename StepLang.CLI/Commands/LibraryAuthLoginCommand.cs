using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
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

    public LibraryAuthLoginCommand(LibApiClient apiClient)
    {
        this.apiClient = apiClient;
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

        await Console.Out.WriteLineAsync();
        await Console.Out.WriteLineAsync("Login successful!");
        await Console.Out.WriteLineAsync();


        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var credentialsPath = new FileInfo(Path.Combine(appData, "STEP", "credentials.json"));

        credentialsPath.Directory!.Create();

        await using var stream = credentialsPath.Create();

        // TODO: standardize this
        // MAYBE: encrypt token with system user account
        var credentials = new Credentials(result.Token!, "https://localhost:7022/");

        await JsonSerializer.SerializeAsync(stream, credentials);

        await Console.Out.WriteLineAsync("Credentials saved to " + credentialsPath.FullName);

        return 0;
    }
}