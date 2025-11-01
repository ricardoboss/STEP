using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tokenizing;

public class Token
{
	public TokenType Type { get; }

	public string Value { get; }

	public TokenLocation Location { get; }

	public Token(TokenType type, string value, TokenLocation? location = null)
	{
		Type = type;
		Value = value;
		Location = location ?? new TokenLocation();
	}

	/// <summary>
	/// The same as <see cref="Value"/>, but if this token is a <see cref="TokenType.LiteralString"/>, the string
	/// without quotes and escape sequences.
	/// </summary>
	public string StringValue => Type switch
	{
		TokenType.LiteralString => Value[1..^1].UnescapeControlChars(), // cut off the quotes, then unescape
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
