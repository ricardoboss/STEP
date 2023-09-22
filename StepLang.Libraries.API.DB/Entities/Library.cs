namespace StepLang.Libraries.API.DB.Entities;

public class Library
{
    public Guid Id { get; set; }

    public Guid AuthorId { get; set; }

    public Author Author { get; set; } = null!;

    public string Name { get; set; } = null!;

    public Guid? LatestVersionId { get; set; }

    public LibraryVersion? LatestVersion { get; set; }

    public ICollection<LibraryVersion> Versions { get; set; } = null!;
}