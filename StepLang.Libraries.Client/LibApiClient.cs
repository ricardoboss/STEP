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

    public async Task<BriefLibraryVersion?> GetLibraryAsync(string name, string? version = null, CancellationToken cancellationToken = default)
    {
        var uri = $"libraries/{name}";
        if (version is not null)
            uri += $"/{version}";

        var response = await httpClient.GetAsync(uri, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<BriefLibraryVersion?>(cancellationToken: cancellationToken);
    }

    public async Task<UploadResult?> UploadLibraryAsync(string name, string version, Stream stream, CancellationToken cancellationToken = default)
    {
        var uri = $"libraries/{name}/{version}";
        var response = await httpClient.PostAsync(uri, new StreamContent(stream), cancellationToken);

        return await response.Content.ReadFromJsonAsync<UploadResult>(cancellationToken: cancellationToken);
    }

    public async Task<RegisterResult?> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("auth/register", request, cancellationToken);

        return await response.Content.ReadFromJsonAsync<RegisterResult>(cancellationToken: cancellationToken);
    }

    public async Task<CreateTokenResult?> CreateTokenAsync(CreateTokenRequest request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("auth/token", request, cancellationToken);

        return await response.Content.ReadFromJsonAsync<CreateTokenResult>(cancellationToken: cancellationToken);
    }

    public async Task<AuthCheckResult?> CheckAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetAsync("auth/check", cancellationToken);

            return await response.Content.ReadFromJsonAsync<AuthCheckResult>(cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            return new("unknown", "Exception occurred while checking authentication: " + e.Message);
        }
    }
}