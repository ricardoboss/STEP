using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Spectre.Console.Cli;
using StepLang.Libraries.API;
using StepLang.Libraries.Client;

namespace StepLang.CLI.Commands;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class LibraryAuthRegisterCommand : AsyncCommand<LibraryAuthRegisterCommand.Settings>
{
    public sealed class Settings : HiddenGlobalCommandSettings
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        await Console.Out.WriteAsync("Username: ");
        var username = Console.ReadLine() ?? throw new InvalidOperationException("Username must be set");

        await Console.Out.WriteAsync("Password: ");
        var password = Console.ReadLine() ?? throw new InvalidOperationException("Password must be set");

        var request = new RegisterRequest(username, password);

        using var httpClient = new HttpClient();
        var apiClient = new LibApiClientFactory(null).CreateClient(httpClient);
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

        await Console.Out.WriteLineAsync();
        await Console.Out.WriteLineAsync("Registration successful!");
        await Console.Out.WriteLineAsync();


        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var credentialsPath = new FileInfo(Path.Combine(appData, "STEP", "credentials.json"));

        credentialsPath.Directory!.Create();

        await using var stream = credentialsPath.Create();

        var credentials = new Credentials(result.Token!, "https://localhost:7022/");

        await JsonSerializer.SerializeAsync(stream, credentials);

        await Console.Out.WriteLineAsync("Credentials saved to " + credentialsPath.FullName);

        return 0;
    }
}