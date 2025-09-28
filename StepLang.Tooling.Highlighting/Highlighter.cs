using StepLang.Tokenizing;
using System.Text;

namespace StepLang.Tooling.Highlighting;

public class Highlighter(ColorScheme scheme)
{
	public IEnumerable<StyledToken> Highlight(string sourceCode)
	{
		var tokenizer = new Tokenizer(sourceCode, []);

		foreach (var token in tokenizer.Tokenize())
		{
			var text = GetTokenText(token);

			yield return new StyledToken(token.Type, text, GetStyle(token.Type, scheme));
		}
	}

	private static string GetTokenText(Token token)
	{
		if (token.Type != TokenType.LiteralString)
			return token.Value;

		var builder = new StringBuilder(token.Value.Length);

		builder.Append('"');

		foreach (var character in token.StringValue)
		{
			switch (character)
			{
				case '\\':
					builder.Append("\\\\");
					break;
				case '"':
					builder.Append("\\\"");
					break;
				case '\n':
					builder.Append("\\n");
					break;
				case '\r':
					builder.Append("\\r");
					break;
				case '\t':
					builder.Append("\\t");
					break;
				case '\0':
					builder.Append("\\0");
					break;
				case '\a':
					builder.Append("\\a");
					break;
				case '\b':
					builder.Append("\\b");
					break;
				case '\f':
					builder.Append("\\f");
					break;
				case '\v':
					builder.Append("\\v");
					break;
				default:
					if (char.IsControl(character))
					{
						builder.Append("\\u");
						builder.Append(((int)character).ToString("x4"));
					}
					else
					{
						builder.Append(character);
					}

					break;
			}
		}

		builder.Append('"');

		return builder.ToString();
	}

	public static Style GetStyle(TokenType tokenType, ColorScheme scheme)
	{
		return tokenType switch
		{
			TokenType.Identifier => scheme.Identifier,
			TokenType.TypeName => scheme.Type,
			TokenType.EqualsSymbol => scheme.Operator,
			TokenType.LiteralString => scheme.String,
			TokenType.LiteralNumber => scheme.Number,
			TokenType.LiteralBoolean => scheme.Bool,
			TokenType.LiteralNull => scheme.Null,
			TokenType.IfKeyword => scheme.Keyword,
			TokenType.ElseKeyword => scheme.Keyword,
			TokenType.WhileKeyword => scheme.Keyword,
			TokenType.BreakKeyword => scheme.Keyword,
			TokenType.ContinueKeyword => scheme.Keyword,
			TokenType.OpeningCurlyBracket => scheme.Punctuation,
			TokenType.ClosingCurlyBracket => scheme.Punctuation,
			TokenType.OpeningParentheses => scheme.Punctuation,
			TokenType.ClosingParentheses => scheme.Punctuation,
			TokenType.CommaSymbol => scheme.Punctuation,
			TokenType.GreaterThanSymbol => scheme.Operator,
			TokenType.LessThanSymbol => scheme.Operator,
			TokenType.PlusSymbol => scheme.Operator,
			TokenType.MinusSymbol => scheme.Operator,
			TokenType.AsteriskSymbol => scheme.Operator,
			TokenType.SlashSymbol => scheme.Operator,
			TokenType.PercentSymbol => scheme.Operator,
			TokenType.PipeSymbol => scheme.Operator,
			TokenType.AmpersandSymbol => scheme.Operator,
			TokenType.ExclamationMarkSymbol => scheme.Operator,
			TokenType.HatSymbol => scheme.Operator,
			TokenType.QuestionMarkSymbol => scheme.Operator,
			TokenType.ReturnKeyword => scheme.Keyword,
			TokenType.UnderscoreSymbol => scheme.Operator,
			TokenType.LineComment => scheme.Comment,
			TokenType.OpeningSquareBracket => scheme.Punctuation,
			TokenType.ClosingSquareBracket => scheme.Punctuation,
			TokenType.ColonSymbol => scheme.Punctuation,
			TokenType.ImportKeyword => scheme.Keyword,
			TokenType.ForEachKeyword => scheme.Keyword,
			TokenType.InKeyword => scheme.Keyword,
			TokenType.Error => scheme.Error,
			_ => scheme.Default,
		};
	}
}
