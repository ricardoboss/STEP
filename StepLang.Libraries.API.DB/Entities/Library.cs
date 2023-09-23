using System.ComponentModel.DataAnnotations.Schema;

namespace StepLang.Libraries.API.DB.Entities;

public class Library
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid? LatestVersionId { get; set; }

    public LibraryVersion? LatestVersion { get; set; }

    [ForeignKey(nameof(LibraryVersion.Library))]
    public ICollection<LibraryVersion> Versions { get; set; } = null!;
}