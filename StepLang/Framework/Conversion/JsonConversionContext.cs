using System.Text.Json.Serialization;
using StepLang.Expressions.Results;

namespace StepLang.Framework.Conversion;

[JsonSerializable(typeof(ExpressionResult))]
public partial class JsonConversionContext : JsonSerializerContext
{
}