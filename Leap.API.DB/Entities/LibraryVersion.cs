namespace Leap.API.DB.Entities;

public class LibraryVersion
{
    public Guid Id { get; set; }

    public Library Library { get; set; } = null!;

    public string Version { get; set; } = null!;

    public ICollection<Library> Dependencies { get; set; } = null!;
}