using Microsoft.Extensions.Http;

namespace StepLang.Libraries.Client;

public class LibApiClientFactory : ITypedHttpClientFactory<LibApiClient>
{
    public LibApiClient CreateClient(HttpClient httpClient)
    {
        httpClient.BaseAddress = new Uri("https://localhost:44388/");

        return new(httpClient);
    }
}