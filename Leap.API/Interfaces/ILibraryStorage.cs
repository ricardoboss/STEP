namespace Leap.API.Interfaces;

public interface ILibraryStorage
{
    public Task<bool> ExistsAsync(string name, string version, CancellationToken cancellationToken = default);

    public Task<Dictionary<string, dynamic>> GetMetadataAsync(string name, string version, CancellationToken cancellationToken = default);

    public Task SetMetadataAsync(string name, string version, Dictionary<string, dynamic> metadata, CancellationToken cancellationToken = default);

    public Task<Stream> OpenReadAsync(string name, string version, CancellationToken cancellationToken = default);

    public Stream OpenWrite(string name, string version, CancellationToken cancellationToken = default);

    public Task DeleteAsync(string name, string version, CancellationToken cancellationToken = default);

    public Task DeleteAsync(string name, CancellationToken cancellationToken = default);
}
