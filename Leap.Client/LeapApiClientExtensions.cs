using Microsoft.Extensions.DependencyInjection;

namespace Leap.Client;

public static class LeapApiClientExtensions
{
    public static IHttpClientBuilder AddLibApiClient(this IServiceCollection services)
    {
        return services
            .AddSingleton<LeapApiClientFactory>()
            .AddHttpClient<LeapApiClient>((sp, http) =>
                sp.GetRequiredService<LeapApiClientFactory>().CreateClient(http));
    }

    public static IServiceCollection AddLibApiCredentialManager(this IServiceCollection services)
    {
        return services.AddSingleton<LeapApiCredentialManager>();
    }
}