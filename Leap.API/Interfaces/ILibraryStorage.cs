namespace Leap.API.Interfaces;

public interface ILibraryStorage
{
    public Task<bool> ExistsAsync(string author, string name, string version, CancellationToken cancellationToken = default);

    public Task<Dictionary<string, dynamic>> GetMetadataAsync(string author, string name, string version, CancellationToken cancellationToken = default);

    public Task SetMetadataAsync(string author, string name, string version, Dictionary<string, dynamic> metadata, CancellationToken cancellationToken = default);

    public Task<Stream> OpenReadAsync(string author, string name, string version, CancellationToken cancellationToken = default);

    public Stream OpenWrite(string author, string name, string version, CancellationToken cancellationToken = default);

    public Task DeleteAsync(string author, string name, string version, CancellationToken cancellationToken = default);

    public Task DeleteAsync(string author, string name, CancellationToken cancellationToken = default);
}
