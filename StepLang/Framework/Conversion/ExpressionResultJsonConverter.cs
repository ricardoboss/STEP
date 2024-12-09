using System.Text.Json;
using System.Text.Json.Serialization;
using StepLang.Expressions.Results;

namespace StepLang.Framework.Conversion;

public class ExpressionResultJsonConverter : JsonConverter<ExpressionResult>
{
	public override ExpressionResult Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (typeToConvert != typeof(ExpressionResult))
		{
			throw new NotSupportedException("Can only convert to ExpressionResult");
		}

		return reader.TokenType switch
		{
			JsonTokenType.False => BoolResult.False,
			JsonTokenType.True => BoolResult.True,
			JsonTokenType.Null => NullResult.Instance,
			JsonTokenType.Number => new NumberResult(reader.GetDouble()),
			JsonTokenType.String => new StringResult(reader.GetString() ?? string.Empty),
			JsonTokenType.StartArray => new ListResult(ReadArray(ref reader)),
			JsonTokenType.StartObject => new MapResult(ReadObject(ref reader)),
			_ => throw new NotSupportedException(
				$"Conversion of {reader.TokenType} to ExpressionResult is not supported"),
		};
	}

	private static List<ExpressionResult> ReadArray(ref Utf8JsonReader reader)
	{
		var results = new List<ExpressionResult>();
		while (reader.Read())
		{
			if (reader.TokenType == JsonTokenType.EndArray)
			{
				return results;
			}

			var value = JsonSerializer.Deserialize(ref reader, JsonConversionContext.Default.ExpressionResult);
			if (value is null)
			{
				throw new JsonException("Unexpected token when reading array.");
			}

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
			{
				return results;
			}

			if (reader.TokenType != JsonTokenType.PropertyName)
			{
				throw new JsonException("Unexpected token when reading object.");
			}

			var key = reader.GetString() ?? string.Empty;
			var value = JsonSerializer.Deserialize(ref reader, JsonConversionContext.Default.ExpressionResult);
			if (value is null)
			{
				throw new JsonException("Unexpected token when reading object.");
			}

			results.Add(key, value);
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
				throw new NotSupportedException($"Conversion of {value} to JSON is not supported");
		}
	}
}
