using Semver;

namespace StepLang.Libraries;

public class LibraryBuilder
{
    private string? name;
    private string? version;
    private string? author;
    private Dictionary<string, string>? dependencies;
    private List<string>? files;

    public static LibraryBuilder From(Library library)
    {
        return new()
        {
            name = library.Name,
            version = library.Version,
            author = library.Author,
            dependencies = library.Dependencies,
            files = library.Files,
        };
    }

    public LibraryBuilder WithName(string newName)
    {
        name = newName;

        return this;
    }

    public LibraryBuilder WithVersion(SemVersion newVersion)
    {
        version = newVersion.ToString();

        return this;
    }

    public LibraryBuilder WithAuthor(string newAuthor)
    {
        author = newAuthor;

        return this;
    }

    public LibraryBuilder WithDependencies(Dictionary<string, string>? newDependencies)
    {
        dependencies = newDependencies;

        return this;
    }

    public LibraryBuilder WithDependency(string dependencyName, SemVersionRange versionRange)
    {
        dependencies ??= new();
        if (!dependencies.TryAdd(dependencyName, versionRange.ToString()!))
            dependencies[dependencyName] = versionRange.ToString()!;

        return this;
    }

    public LibraryBuilder WithFiles(IEnumerable<string>? newFiles)
    {
        files = newFiles?.ToList();

        return this;
    }

    public LibraryBuilder WithFile(string file)
    {
        files ??= new();
        files.Add(file);

        return this;
    }

    public Library Build()
    {
        if (name is null)
            throw new InvalidOperationException("Name must be set");

        return new(name, version, author, dependencies, files);
    }
}