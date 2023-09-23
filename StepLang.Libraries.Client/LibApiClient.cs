using System.Net;
using System.Net.Http.Json;
using StepLang.Libraries.API;

namespace StepLang.Libraries.Client;

public class LibApiClient
{
    private readonly HttpClient httpClient;

    public LibApiClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<BriefLibraryDto?> GetLibraryAsync(string name, string? version = null)
    {
        var uri = $"libraries/{name}";
        if (version is not null)
            uri += $"/{version}";

        var response = await httpClient.GetAsync(uri);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<BriefLibraryDto?>();
    }
}