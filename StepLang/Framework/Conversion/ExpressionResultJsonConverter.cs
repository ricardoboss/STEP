using System.Text.Json;
using System.Text.Json.Serialization;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Conversion;

public class ExpressionResultJsonConverter : JsonConverter<ExpressionResult>
{
    public override ExpressionResult Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert != typeof(ExpressionResult))
            throw new NotImplementedException("Can only convert to ExpressionResult");

        return reader.TokenType switch
        {
            JsonTokenType.False => BoolResult.False,
            JsonTokenType.True => BoolResult.True,
            JsonTokenType.Null => NullResult.Instance,
            JsonTokenType.Number => new NumberResult(reader.GetDouble()),
            JsonTokenType.String => new StringResult(reader.GetString() ?? string.Empty),
            JsonTokenType.StartArray => new ListResult(ReadArray(ref reader, options)),
            JsonTokenType.StartObject => new MapResult(ReadObject(ref reader, options)),
            _ => throw new NotImplementedException($"Conversion of {reader.TokenType} to ExpressionResult is not implemented"),
        };
    }

    private static IList<ExpressionResult> ReadArray(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var results = new List<ExpressionResult>();
        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.EndArray:
                    return results;
                default:
                    results.Add(JsonSerializer.Deserialize<ExpressionResult>(ref reader, options)!);
                    break;
            }
        }

        throw new JsonException("Unexpected end of JSON while reading array.");
    }

    private static IDictionary<string, ExpressionResult> ReadObject(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var results = new Dictionary<string, ExpressionResult>();
        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.EndObject:
                    return results;
                case JsonTokenType.PropertyName:
                    var key = reader.GetString() ?? string.Empty;
                    var value = JsonSerializer.Deserialize<ExpressionResult>(ref reader, options)!;

                    results.Add(key, value);

                    break;
                default:
                    throw new JsonException("Unexpected token when reading object.");
            }
        }

        throw new JsonException("Unexpected end of JSON while reading object.");
    }

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
                throw new NotImplementedException($"Conversion of {value} to JSON is not implemented");
        }
    }
}