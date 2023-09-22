namespace StepLang.Libraries.API.DB.Entities;

public class LibraryVersionDependency
{
    public Guid VersionId { get; set; }

    public LibraryVersion Version { get; set; } = null!;

    public Guid DependencyId { get; set; }

    public Library Dependency { get; set; } = null!;
}