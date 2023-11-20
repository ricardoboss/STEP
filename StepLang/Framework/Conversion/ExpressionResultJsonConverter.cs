using System.Text.Json;
using System.Text.Json.Serialization;
using StepLang.Expressions.Results;

namespace StepLang.Framework.Conversion;

/// <summary>
/// Converts <see cref="ExpressionResult"/> to and from JSON.
/// </summary>
/// <seealso cref="JsonConversionContext"/>
public class ExpressionResultJsonConverter : JsonConverter<ExpressionResult>
{
    /// <summary>
    /// Reads a JSON value and converts it to an <see cref="ExpressionResult"/>.
    /// </summary>
    /// <param name="reader">The reader to read from.</param>
    /// <param name="typeToConvert">The type to convert to.</param>
    /// <param name="options">The options to use.</param>
    /// <returns>The converted <see cref="ExpressionResult"/>.</returns>
    /// <exception cref="NotSupportedException">Thrown if <paramref name="typeToConvert"/> is not <see cref="ExpressionResult"/>.</exception>
    public override ExpressionResult Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert != typeof(ExpressionResult))
            throw new NotSupportedException("Can only convert to ExpressionResult");

        return reader.TokenType switch
        {
            JsonTokenType.False => BoolResult.False,
            JsonTokenType.True => BoolResult.True,
            JsonTokenType.Null => NullResult.Instance,
            JsonTokenType.Number => new NumberResult(reader.GetDouble()),
            JsonTokenType.String => new StringResult(reader.GetString() ?? string.Empty),
            JsonTokenType.StartArray => new ListResult(ReadArray(ref reader)),
            JsonTokenType.StartObject => new MapResult(ReadObject(ref reader)),
            _ => throw new NotSupportedException($"Conversion of {reader.TokenType} to ExpressionResult is not supported"),
        };
    }

    private static List<ExpressionResult> ReadArray(ref Utf8JsonReader reader)
    {
        var results = new List<ExpressionResult>();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
                return results;

            var value = JsonSerializer.Deserialize(ref reader, JsonConversionContext.Default.ExpressionResult);
            if (value is null)
                throw new JsonException("Unexpected token when reading array.");

            results.Add(value);
        }

        throw new JsonException("Unexpected end of JSON while reading array.");
    }

    private static Dictionary<string, ExpressionResult> ReadObject(ref Utf8JsonReader reader)
    {
        var results = new Dictionary<string, ExpressionResult>();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                return results;

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException("Unexpected token when reading object.");

            var key = reader.GetString() ?? string.Empty;
            var value = JsonSerializer.Deserialize(ref reader, JsonConversionContext.Default.ExpressionResult);
            if (value is null)
                throw new JsonException("Unexpected token when reading object.");

            results.Add(key, value);
        }

        throw new JsonException("Unexpected end of JSON while reading object.");
    }

    /// <summary>
    /// Writes an <see cref="ExpressionResult"/> to JSON.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="options">The options to use.</param>
    /// <exception cref="NotSupportedException">Thrown if <paramref name="value"/> is not a supported type.</exception>
    public override void Write(Utf8JsonWriter writer, ExpressionResult value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case StringResult { Value: var stringValue }:
                writer.WriteStringValue(stringValue);
                return;
            case NumberResult { Value: var numberValue }:
                writer.WriteNumberValue(numberValue);
                return;
            case BoolResult { Value: var boolValue }:
                writer.WriteBooleanValue(boolValue);
                return;
            case NullResult:
                writer.WriteNullValue();
                return;
            case ListResult { Value: var listValue }:
                writer.WriteStartArray();
                foreach (var item in listValue)
                {
                    Write(writer, item, options);
                }
                writer.WriteEndArray();
                return;
            case MapResult { Value: var mapValue }:
                writer.WriteStartObject();
                foreach (var (key, item) in mapValue)
                {
                    writer.WritePropertyName(key);
                    Write(writer, item, options);
                }
                writer.WriteEndObject();
                return;
            default:
                throw new NotSupportedException($"Conversion of {value} to JSON is not supported");
        }
    }
}