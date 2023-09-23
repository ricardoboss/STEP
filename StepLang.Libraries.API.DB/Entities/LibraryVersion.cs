using System.ComponentModel.DataAnnotations.Schema;

namespace StepLang.Libraries.API.DB.Entities;

public class LibraryVersion
{
    public Guid Id { get; set; }

    public Library Library { get; set; } = null!;

    public string Version { get; set; } = null!;

    public string LibraryJson { get; set; } = null!;

    [ForeignKey("DependencyId")]
    public ICollection<Library> Dependencies { get; set; } = null!;

    [ForeignKey("DependentId")]
    public ICollection<Library> Dependents { get; set; } = null!;
}