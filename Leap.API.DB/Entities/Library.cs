using System.ComponentModel.DataAnnotations.Schema;

namespace Leap.API.DB.Entities;

public class Library
{
    public Guid Id { get; set; }

    public string Author { get; set; } = null!;

    public string Name { get; set; } = null!;

    [ForeignKey("LatestVersionId")]
    public LibraryVersion? LatestVersion { get; set; }

    public ICollection<Author> Maintainers { get; set; } = null!;

    public ICollection<LibraryVersion> Versions { get; set; } = null!;

    public ICollection<LibraryVersionDependency> Dependents { get; set; } = null!;

    /// <inheritdoc />
    public override string ToString() => $"Library(Id={Id}, Author={Author}, Name={Name}, LatestVersion={LatestVersion?.Version})";
}