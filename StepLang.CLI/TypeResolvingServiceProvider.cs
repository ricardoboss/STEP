using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace StepLang.CLI;

public sealed class TypeResolvingServiceProvider : IServiceProvider, ITypeResolver, IDisposable, IAsyncDisposable
{
    private readonly ServiceProvider serviceProvider;

    public TypeResolvingServiceProvider(IServiceCollection serviceCollection)
    {
        serviceProvider = serviceCollection.BuildServiceProvider();
    }

    public object? Resolve(Type? type)
    {
        return type is null ? null : serviceProvider.GetService(type);
    }

    public object? GetService(Type serviceType)
    {
        return serviceProvider.GetService(serviceType);
    }

    public void Dispose()
    {
        serviceProvider.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await serviceProvider.DisposeAsync();
    }
}