using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Http;

namespace Leap.Client;

public class LeapApiClientFactory : ITypedHttpClientFactory<LeapApiClient>
{
    public const string DefaultApiBaseAddress = "https://lib.step-lang.dev/api/";

    private readonly LeapApiCredentialManager? credentialManager;
    private readonly IConfiguration configuration;

    public LeapApiClientFactory(LeapApiCredentialManager? credentialManager, IConfiguration configuration)
    {
        this.credentialManager = credentialManager;
        this.configuration = configuration;
    }

    private string? GetConfiguredBaseAddress() => configuration["LibApi:BaseAddress"];

    public LeapApiClient CreateClient(HttpClient httpClient)
    {
        var credentials = credentialManager?.TryReadCredentials();

        httpClient.BaseAddress = new(GetConfiguredBaseAddress() ?? credentials?.BaseAddress ?? DefaultApiBaseAddress);

        if (credentials?.Token is { } token)
            httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);

        return new(httpClient);
    }
}