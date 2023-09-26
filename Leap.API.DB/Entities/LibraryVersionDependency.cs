namespace Leap.API.DB.Entities;

public class LibraryVersionDependency
{
    public Guid VersionId { get; set; }

    public LibraryVersion Version { get; set; } = null!;

    public Guid DependencyId { get; set; }

    public Library Dependency { get; set; } = null!;

    public string VersionRange { get; set; } = null!;

    /// <inheritdoc />
    public override string ToString() => $"LibraryVersionDependency(Version={VersionId}, Dependency={DependencyId}, VersionRange={VersionRange})";
}