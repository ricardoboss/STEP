using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace StepLang.Tokenizing;

public static partial class StringExtensions
{
	public static bool IsValidIdentifier(this string value)
	{
		return value.Trim().Length != 0 && !value.GetFirstInvalidIdentifierCharacter().HasValue;
	}

	public static char? GetFirstInvalidIdentifierCharacter(this string value)
	{
		if (value.Trim().Length == 0)
		{
			throw new InvalidOperationException(
				"Cannot get the first invalid identifier character of an empty string.");
		}

		if (value.Contains('.', StringComparison.InvariantCulture))
		{
			return '.';
		}

		var firstChar = value[0];
		if (!char.IsAsciiLetter(firstChar) && firstChar != '_')
		{
			return firstChar;
		}

		var invalidChar = value[1..].FirstOrDefault(c => !char.IsAsciiLetterOrDigit(c) && c != '_');

		return invalidChar == 0 ? null : invalidChar;
	}

	[return: NotNullIfNotNull(nameof(value))]
	public static string? EscapeControlCharacters(this string? value)
	{
		if (value is null)
			return null;

		if (value.Length == 0)
			return string.Empty;

		var sb = new StringBuilder(value.Length);
		foreach (var c in value)
		{
			switch (c)
			{
				case '\n': sb.Append("\\n"); break;
				case '\r': sb.Append("\\r"); break;
				case '\t': sb.Append("\\t"); break;
				case '\\': sb.Append(@"\\"); break;
				case '\"': sb.Append("\\\""); break;
				case '\'': sb.Append("\\'"); break;
				default:
					if (c is <= '\x1F' or '\x7F')
						sb.Append("\\x").Append(((int)c).ToString("X2"));
					else
						sb.Append(c);

					break;
			}
		}

		return sb.ToString();
	}

	[return: NotNullIfNotNull(nameof(value))]
	public static string? UnescapeControlChars(this string? value)
	{
		if (value is null)
			return null;

		if (value.Length == 0)
			return string.Empty;

		return EscapedControlCharacterRegex().Replace(value, m =>
		{
			var token = m.Groups[1].Value;
			return token switch
			{
				"\\" => "\\",
				"\"" => "\"",
				"'" => "'",
				"n" => "\n",
				"r" => "\r",
				"t" => "\t",
				_ when token.StartsWith('x') || token.StartsWith('X')
					=> ((char)Convert.ToInt32(token[1..], 16)).ToString(),
				_ => m.Value,
			};
		});
	}

	[GeneratedRegex(@"\\(\\|""|'|n|r|t|x[0-9A-Fa-f]{2})")]
	private static partial Regex EscapedControlCharacterRegex();
}
