using System.Text.Json;
using Microsoft.Extensions.Http;

namespace StepLang.Libraries.Client;

public class LibApiClientFactory : ITypedHttpClientFactory<LibApiClient>
{
    private readonly Credentials? credentials;

    public LibApiClientFactory(Credentials? credentials)
    {
        this.credentials = credentials;
    }

    public LibApiClient CreateClient(HttpClient httpClient)
    {
        var c = credentials ?? TryReadCredentials().Result;

        httpClient.BaseAddress = new(c?.DefaultAddress ?? "https://localhost:7022/");

        if (c?.Token is { } token)
            httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);

        return new(httpClient);
    }

    private static async Task<Credentials?> TryReadCredentials()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var credentialsPath = Path.Combine(appData, "STEP", "credentials.json");

        if (!File.Exists(credentialsPath))
            return null;

        await using var stream = File.OpenRead(credentialsPath);

        return await JsonSerializer.DeserializeAsync<Credentials>(stream);
    }
}