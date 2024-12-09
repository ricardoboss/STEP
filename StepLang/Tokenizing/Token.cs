using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tokenizing;

public class Token(TokenType type, string value, TokenLocation? location = null)
{
	public TokenType Type { get; } = type;

	public string Value { get; } = value;

	public TokenLocation Location { get; } = location ?? new TokenLocation();

	public string StringValue => Type switch
	{
		TokenType.LiteralString => Value[1..^1], // cut off the quotes
		_ => Value,
	};

	[ExcludeFromCodeCoverage]
	public override string ToString()
	{
		var printableValue = Value
			.Replace("\n", "\\n", StringComparison.InvariantCulture)
			.Replace("\r", "\\r", StringComparison.InvariantCulture);

		return Value.Length > 0 ? $"<{Type}: '{printableValue}'>" : $"<{Type}>";
	}
}
