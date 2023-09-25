namespace Leap.API.DB.Entities;

public class Library
{
    public Guid Id { get; set; }

    public Guid MaintainerId { get; set; }

    public Author Maintainer { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string Name { get; set; } = null!;

    public Guid? LatestVersionId { get; set; }

    public LibraryVersion? LatestVersion { get; set; }

    public ICollection<LibraryVersion> Versions { get; set; } = null!;
}