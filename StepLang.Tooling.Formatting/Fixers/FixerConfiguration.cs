using System.Text;
using System.Text.Json;

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

    public LineEndings LineEnding { get; set; } = LineEndings.Lf;

    public string Encoding { get; set; } = "utf-8";

    public Indentations Indentation { get; set; } = Indentations.Tab;

    public int IndentationSize { get; set; } = 4;

    public Encoding GetParsedEncoding() => System.Text.Encoding.GetEncoding(Encoding);

    public enum LineEndings
    {
        Lf,
        CrLf,
    }

    public enum Indentations
    {
        Tab,
        Space,
    }
}