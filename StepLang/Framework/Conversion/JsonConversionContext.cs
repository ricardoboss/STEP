using System.Text.Json.Serialization;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Conversion;

[JsonSerializable(typeof(ExpressionResult))]
public partial class JsonConversionContext : JsonSerializerContext
{
}