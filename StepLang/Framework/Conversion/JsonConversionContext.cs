using System.Text.Json.Serialization;
using StepLang.Expressions.Results;

namespace StepLang.Framework.Conversion;

/// <summary>
/// <para>
/// The <see cref="JsonSerializerContext"/> for StepLang.
/// </para>
/// <para>
/// This class generates the sources required for faster JSON serialization and deserialization of <see cref="ExpressionResult"/>.
/// </para>
/// </summary>
/// <seealso cref="JsonSerializableAttribute"/>
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(ExpressionResult))]
public partial class JsonConversionContext : JsonSerializerContext;
