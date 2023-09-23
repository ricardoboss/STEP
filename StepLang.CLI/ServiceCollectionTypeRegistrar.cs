using System.Collections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Spectre.Console.Cli;

namespace StepLang.CLI;

public sealed class ServiceCollectionTypeRegistrar : IServiceCollection, ITypeRegistrar, IDisposable, IAsyncDisposable
{
    private readonly ServiceCollection services = new();

    private TypeResolvingServiceProvider? builtResolver;

    public void Register(Type service, Type implementation)
    {
        services.AddTransient(service, implementation);
    }

    public void RegisterInstance(Type service, object implementation)
    {
        services.AddSingleton(service, implementation);
    }

    public void RegisterLazy(Type service, Func<object> factory)
    {
        services.AddTransient(service, _ => factory());
    }

    public ITypeResolver Build()
    {
        builtResolver?.Dispose();

        return builtResolver = new(services);
    }

    public void Dispose()
    {
        builtResolver?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (builtResolver != null)
            await builtResolver.DisposeAsync();
    }

    public IEnumerator<ServiceDescriptor> GetEnumerator()
    {
        return services.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)services).GetEnumerator();
    }

    public void Add(ServiceDescriptor item)
    {
        services.Add(item);
    }

    public void Clear()
    {
        services.Clear();
    }

    public bool Contains(ServiceDescriptor item)
    {
        return services.Contains(item);
    }

    public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
    {
        services.CopyTo(array, arrayIndex);
    }

    public bool Remove(ServiceDescriptor item)
    {
        return services.Remove(item);
    }

    public int Count => services.Count;

    public bool IsReadOnly => services.IsReadOnly;

    public int IndexOf(ServiceDescriptor item)
    {
        return services.IndexOf(item);
    }

    public void Insert(int index, ServiceDescriptor item)
    {
        services.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        services.RemoveAt(index);
    }

    public ServiceDescriptor this[int index]
    {
        get => services[index];
        set => services[index] = value;
    }
}