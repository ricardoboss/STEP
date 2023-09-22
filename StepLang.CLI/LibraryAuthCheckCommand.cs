using StepLang.Libraries.Client;

namespace StepLang.CLI;

public static class LibraryAuthCheckCommand
{
    public static async Task<int> Invoke()
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