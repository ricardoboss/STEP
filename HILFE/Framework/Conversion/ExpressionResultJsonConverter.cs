using System.Text.Json;
using System.Text.Json.Serialization;
using HILFE.Parsing.Expressions;

namespace HILFE.Framework.Conversion;

public class ExpressionResultJsonConverter : JsonConverter<ExpressionResult>
{
    public override ExpressionResult Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert != typeof(ExpressionResult))
            throw new NotImplementedException("Can only convert to ExpressionResult");

        return reader.TokenType switch
        {
            JsonTokenType.False => ExpressionResult.False,
            JsonTokenType.True => ExpressionResult.True,
            JsonTokenType.Null => ExpressionResult.Null,
            JsonTokenType.Number => ExpressionResult.Number(reader.GetDouble()),
            JsonTokenType.String => ExpressionResult.String(reader.GetString() ?? string.Empty),
            JsonTokenType.StartArray => ExpressionResult.List(ReadArray(ref reader, options)),
            _ => throw new NotImplementedException($"Conversion of {reader.TokenType} to ExpressionResult is not implemented"),
        };
    }

    private static IEnumerable<ExpressionResult> ReadArray(ref Utf8JsonReader reader, JsonSerializerOptions options)
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

    public override void Write(Utf8JsonWriter writer, ExpressionResult value, JsonSerializerOptions options)
    {
        switch (value.ValueType)
        {
            case "string" when value.Value is string stringValue:
                writer.WriteStringValue(stringValue);
                break;
            case "number" when value.Value is double numberValue:
                writer.WriteNumberValue(numberValue);
                break;
            case "bool" when value.Value is bool boolValue:
                writer.WriteBooleanValue(boolValue);
                break;
            case "null":
                writer.WriteNullValue();
                break;
            case "list" when value.Value is IEnumerable<ExpressionResult> listValue:
                writer.WriteStartArray();
                foreach (var item in listValue)
                {
                    Write(writer, item, options);
                }
                writer.WriteEndArray();
                break;
            default:
                throw new NotImplementedException($"Conversion of {value} to JSON is not implemented");
        }
    }
}