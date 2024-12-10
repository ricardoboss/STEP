using StepLang.Expressions.Results;
using System.Text.Json.Serialization;

namespace StepLang.Framework.Conversion;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(ExpressionResult))]
public partial class JsonConversionContext : JsonSerializerContext;
