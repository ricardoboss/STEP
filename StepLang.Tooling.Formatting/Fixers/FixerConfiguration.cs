using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StepLang.Tooling.Formatting.Fixers;

public class FixerConfiguration
{
    public static FixerConfiguration? FromFile(FileInfo file)
    {
        return file.Exists ? FromJson(File.ReadAllText(file.FullName)) : null;
    }

    public static FixerConfiguration? FromJson(string json)
    {
        return JsonSerializer.Deserialize(json, FixerConfigurationJsonContext.Default.FixerConfiguration);
    }

    public string LineEndings { get; set; } = "\n";

    public string Encoding { get; set; } = "utf-8";

    public bool TabIndentation { get; set; } = true;

    public int IndentationSize { get; set; } = 4;

    private Encoding? parsedEncoding;

    [JsonIgnore]
    public Encoding ParsedEncoding => parsedEncoding ??= System.Text.Encoding.GetEncoding(Encoding);
}