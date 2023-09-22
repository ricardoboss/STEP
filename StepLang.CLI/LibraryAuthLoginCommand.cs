using System.Text.Json;
using StepLang.Libraries.API;
using StepLang.Libraries.Client;

namespace StepLang.CLI;

public static class LibraryAuthLoginCommand
{
    public static async Task<int> Invoke()
    {
        await Console.Out.WriteAsync("Username: ");
        var username = Console.ReadLine() ?? throw new InvalidOperationException("Username must be set");

        await Console.Out.WriteAsync("Password: ");
        var password = Console.ReadLine() ?? throw new InvalidOperationException("Password must be set");

        var request = new CreateTokenRequest(username, password);

        using var httpClient = new HttpClient();
        var apiClient = new LibApiClientFactory(null).CreateClient(httpClient);
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

        var credentials = new Credentials(result.Token!, "https://localhost:7022/");

        await JsonSerializer.SerializeAsync(stream, credentials);

        await Console.Out.WriteLineAsync("Credentials saved to " + credentialsPath.FullName);

        return 0;
    }
}