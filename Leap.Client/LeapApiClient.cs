using System.Net.Http.Json;
using System.Text.Json;
using Leap.Common.API;

namespace Leap.Client;

public class LeapApiClient
{
    private readonly HttpClient httpClient;

    public LeapApiClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<BriefLibraryVersion?> GetLibraryAsync(string author, string name, string? version = null, CancellationToken cancellationToken = default)
    {
        var uri = $"libraries/{author}/{name}";
        if (version is not null)
            uri += $"/{version}";

        try
        {
            return await GetJsonAsync<BriefLibraryVersion?>(uri, cancellationToken);
        }
        catch (LeapApiException)
        {
            return null;
        }
    }

    public async Task<UploadResult?> UploadLibraryAsync(string author, string name, string version, Stream stream, CancellationToken cancellationToken = default)
    {
        var uri = $"libraries/{author}/{name}/{version}";

        return await PostStreamAsync<UploadResult>(uri, stream, cancellationToken);
    }

    public async Task<RegisterResult?> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        return await PostJsonAsync<RegisterResult, RegisterRequest>("auth/register", request, cancellationToken);
    }

    public async Task<CreateTokenResult?> CreateTokenAsync(CreateTokenRequest request, CancellationToken cancellationToken = default)
    {
        return await PostJsonAsync<CreateTokenResult, CreateTokenRequest>("auth/token", request, cancellationToken);
    }

    public async Task<AuthCheckResult?> CheckAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await GetJsonAsync<AuthCheckResult>("auth/check", cancellationToken);
        }
        catch (LeapApiException e)
        {
            return new("unknown", e.Message);
        }
    }

    private async Task<T?> GetJsonAsync<T>(string uri, CancellationToken cancellationToken = default)
    {
        return await WrapAsync(async () =>
        {
            var response = await httpClient.GetAsync(uri, cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
        }, cancellationToken);
    }

    private async Task<TResponse?> PostJsonAsync<TResponse, TRequest>(string uri, TRequest? content, CancellationToken cancellationToken = default)
    {
        return await WrapAsync(async () =>
        {
            var response = await httpClient.PostAsJsonAsync(uri, content, cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
        }, cancellationToken);
    }

    private async Task<T?> PostStreamAsync<T>(string uri, Stream stream, CancellationToken cancellationToken = default)
    {
        return await WrapAsync(async () =>
        {
            var response = await httpClient.PostAsync(uri, new StreamContent(stream), cancellationToken);

            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);

            return body;
        }, cancellationToken);
    }

    private static async Task<T> WrapAsync<T>(Func<Task<T>> func, CancellationToken cancellationToken = default)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await func();
        }
        catch (HttpRequestException e)
        {
            throw new LeapApiException($"Request failed with status {e.StatusCode}", e);
        }
        catch (JsonException e)
        {
            throw new LeapApiException("Failed to parse response.", e);
        }
        catch (Exception e)
        {
            throw new LeapApiException("An unknown error occurred.", e);
        }
    }
}