using Semver;

namespace StepLang.Libraries;

public class LibraryBuilder
{
    private string? name;
    private string? version;
    private string? author;
    private List<Dependency>? dependencies;
    private List<string>? files;

    public static LibraryBuilder From(Library library)
    {
        return new()
        {
            name = library.Name,
            version = library.Version,
            author = library.Author,
            dependencies = library.Dependencies?.ToList(),
            files = library.Files?.ToList(),
        };
    }

    public LibraryBuilder WithName(string name)
    {
        this.name = name;

        return this;
    }

    public LibraryBuilder WithVersion(SemVersion version)
    {
        this.version = version.ToString();

        return this;
    }

    public LibraryBuilder WithAuthor(string author)
    {
        this.author = author;

        return this;
    }

    public LibraryBuilder WithDependencies(IEnumerable<Dependency>? dependencies)
    {
        this.dependencies = dependencies?.ToList();

        return this;
    }

    public LibraryBuilder WithDependency(Dependency dependency)
    {
        dependencies ??= new();
        dependencies.Add(dependency);

        return this;
    }

    public LibraryBuilder WithFiles(IEnumerable<string>? files)
    {
        this.files = files?.ToList();

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