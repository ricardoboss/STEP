using Microsoft.Extensions.DependencyInjection;

namespace StepLang.Libraries.Client;

public static class LibApiClientExtensions
{
    public static IHttpClientBuilder AddLibApiClient(this IServiceCollection services)
    {
        return services
            .AddSingleton<LibApiClientFactory>()
            .AddHttpClient<LibApiClient>((sp, http) =>
                sp.GetRequiredService<LibApiClientFactory>().CreateClient(http));
    }

    public static IServiceCollection AddLibApiCredentialManager(this IServiceCollection services)
    {
        return services.AddSingleton<LibApiCredentialManager>();
    }
}