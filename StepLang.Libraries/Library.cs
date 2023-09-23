using System.Text.Json;
using System.Text.Json.Serialization;

namespace StepLang.Libraries;

public record Library(string Name, string? Version, string? Author, Dictionary<string, string>? Dependencies,
    List<string>? Files)
{
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
        return JsonSerializer.Deserialize<Library>(json);
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
