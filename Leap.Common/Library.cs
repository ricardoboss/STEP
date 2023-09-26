using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Leap.Common;

public partial record Library(string Name, string? Version, string? Author, Dictionary<string, string>? Dependencies,
    List<string>? Files)
{
    [GeneratedRegex("[a-z][-a-z]{0,16}[a-z]")]
    public static partial Regex UsernameRegex();

    [GeneratedRegex("[a-z][-a-z]{0,16}[a-z]")]
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
            throw new ValidationException("User name must only contain lowercase characters and hyphens (-) and must start and end with a character.");

        if (!NameRegex().IsMatch(name))
            throw new ValidationException("Library name must only contain lowercase characters and hyphens (-) and must start and end with a character");
    }

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

    [JsonIgnore]
    public string NameAuthorPart => Name.Split('/')[0];

    [JsonIgnore]
    public string NameLibraryPart => Name.Split('/')[1];
}
