namespace Leap.API.DB.Entities;

public class LibraryVersion
{
    public Guid Id { get; set; }

    public Guid LibraryId { get; set; }

    public Library Library { get; set; } = null!;

    public string Version { get; set; } = null!;

    public ICollection<LibraryVersionDependency> Dependencies { get; set; } = null!;

    /// <inheritdoc />
    public override string ToString() => $"LibraryVersion(Id={Id}, Library={Library.Author}/{Library.Name}, Version={Version})";
}