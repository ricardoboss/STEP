using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Http;

namespace StepLang.Libraries.Client;

public class LibApiClientFactory : ITypedHttpClientFactory<LibApiClient>
{
    public const string DefaultApiBaseAddress = "https://lib.step-lang.dev/api/";

    private readonly LibApiCredentialManager? credentialManager;
    private readonly IConfiguration configuration;

    public LibApiClientFactory(LibApiCredentialManager? credentialManager, IConfiguration configuration)
    {
        this.credentialManager = credentialManager;
        this.configuration = configuration;
    }

    private string? GetConfiguredBaseAddress() => configuration["LibApi:BaseAddress"];

    public LibApiClient CreateClient(HttpClient httpClient)
    {
        var credentials = credentialManager?.TryReadCredentials();

        httpClient.BaseAddress = new(GetConfiguredBaseAddress() ?? credentials?.BaseAddress ?? DefaultApiBaseAddress);

        if (credentials?.Token is { } token)
            httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);

        return new(httpClient);
    }
}