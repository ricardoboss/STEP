using StepLang.Libraries.API.Interfaces;

namespace StepLang.Libraries.API.Extensions;

public static class ILibraryStorageExtensions
{
    public static async Task UpdateMetadataAsync(this ILibraryStorage storage, string name, string version, Action<Dictionary<string, dynamic>> callback, CancellationToken cancellationToken = default)
    {
        var metadata = await storage.GetMetadataAsync(name, version, cancellationToken);
        callback(metadata);
        await storage.SetMetadataAsync(name, version, metadata, cancellationToken);
    }
}