using System.Text.Json.Serialization;
using StepLang.Expressions.Results;

namespace StepLang.Framework.Conversion;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(ExpressionResult))]
public partial class JsonConversionContext : JsonSerializerContext;
