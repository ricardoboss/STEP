using Leap.API.Services;

namespace Leap.API.Interfaces;

public interface ILibraryStorage
{
    public Task<bool> ExistsAsync(string author, string name, string version, CancellationToken cancellationToken = default);

    public Task<StorageMetadata?> GetMetadataAsync(string author, string name, string version, CancellationToken cancellationToken = default);

    public Task SetMetadataAsync(string author, string name, string version, StorageMetadata metadata, CancellationToken cancellationToken = default);

    public Task<Stream> OpenReadAsync(string author, string name, string version, CancellationToken cancellationToken = default);

    public Stream OpenWrite(string author, string name, string version, CancellationToken cancellationToken = default);

    public Task DeleteAsync(string author, string name, string version, CancellationToken cancellationToken = default);

    public Task DeleteAsync(string author, string name, CancellationToken cancellationToken = default);
}
