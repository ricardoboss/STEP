using System.Text.Json.Serialization;

namespace StepLang.Tooling.Formatting.Fixers;

[JsonSerializable(typeof(FixerConfiguration))]
public partial class FixerConfigurationJsonContext : JsonSerializerContext
{
}