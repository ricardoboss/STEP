using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Leap.Common;

public partial class Library
{
    [JsonIgnore]
    public string NameAuthorPart => GetAuthorPart(Name);

    [JsonIgnore]
    public string NameLibraryPart => GetLibraryPart(Name);

    public string Name { get; set; }

    public string? Version { get; set; }

    public string? Author { get; set; }

    public Dictionary<string, string>? Dependencies { get; set; }

    public List<string>? Files { get; set; }

    public Library(string name, string? version, string? author, Dictionary<string, string>? dependencies, List<string>? files)
    {
        Name = name;
        Version = version;
        Author = author;
        Dependencies = dependencies;
        Files = files;
    }

    [GeneratedRegex("^[a-z][-a-z]{0,16}[a-z]$")]
    public static partial Regex UsernameRegex();

    [GeneratedRegex("^[a-z][-a-z]{0,16}[a-z]$")]
    public static partial Regex NameRegex();

    public static bool IsCurrentDirLibrary()
    {
        return File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "library.json"));
    }

    public static Library FromCurrentDir()
    {
        return FromPath(Path.Combine(Directory.GetCurrentDirectory(), "library.json"));
    }

    public static Library FromPath(string path)
    {
        if (!File.Exists(path))
            throw new InvalidOperationException($"{path} does not exist");

        var libraryJson = File.ReadAllText(path);

        return FromJson(libraryJson) ?? throw new InvalidOperationException($"Unable to read {path}");
    }

    public static Library? FromJson(string json)
    {
        return JsonSerializer.Deserialize<Library>(json)?.Validate();
    }

    public static void ValidateName(string libraryName)
    {
        var parts = libraryName.Split('/');
        if (parts.Length != 2)
            throw new ValidationException("Library name must match '<author>/<library>' pattern");

        var (author, name) = (parts[0], parts[1]);

        if (!UsernameRegex().IsMatch(author))
            throw new ValidationException("Author must only contain lowercase characters and hyphens (-) and must start and end with a character.");

        if (!NameRegex().IsMatch(name))
            throw new ValidationException("Library name must only contain lowercase characters and hyphens (-) and must start and end with a character");
    }

    public static string GetAuthorPart(string name) => name.Split('/')[0];

    public static string GetLibraryPart(string name) => name.Split('/')[0];

    public Library Validate()
    {
        ValidateName(Name);

        return this;
    }

    public LibraryBuilder Modify()
    {
        return LibraryBuilder.From(this);
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        });
    }

    public Library SaveTo(string path)
    {
        File.WriteAllText(path, ToJson());

        return this;
    }

    public Library SaveToCurrentDir()
    {
        return SaveTo(Path.Combine(Directory.GetCurrentDirectory(), "library.json"));
    }
}
