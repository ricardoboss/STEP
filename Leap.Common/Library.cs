using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Leap.Common;

public partial record Library(string Name, string? Version, string? Author, Dictionary<string, string>? Dependencies,
    List<string>? Files)
{
    [GeneratedRegex("[a-z][a-z-]{0,16}[a-z]/[a-z][a-z-]{0,16}[a-z]")]
    private static partial Regex LibraryNameRegex();

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

    public static void ValidateName(string name)
    {
        if (name.Split('/').Length != 2)
            throw new ArgumentException("Library name must match '<user>/<library>' pattern", nameof(name));

        if (!LibraryNameRegex().IsMatch(name))
            throw new ArgumentException("Library name must only contain lowercase characters and hyphens (-) and must start and end with a character", nameof(name));
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
}
