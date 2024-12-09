using StepLang.Tokenizing;

namespace StepLang.Parsing;

public class UnexpectedTokenException(Token token, string message) : ParserException(1, token, message, BuildHelpText())
{
	private static string BuildMessage(Token token, TokenType[] allowed)
	{
		var expectation = allowed.Length switch
		{
			0 => "nothing",
			1 => $"a {allowed[0].ToDisplay()}",
			_ => $"one of {string.Join(", ", allowed[..^1].Select(TokenTypes.ToDisplay))} or {allowed[^1].ToDisplay()}",
		};

		return $"Expected {expectation}, but got {token.Type.ToDisplay()} instead";
	}

	private static string BuildHelpText()
	{
		return
			"This error is usually caused by a missing token or a typo. Check the syntax of the statement you are trying to use. Also review your code for syntax errors, such as missing parentheses or other punctuation.";
	}

	public UnexpectedTokenException(Token token, params TokenType[] allowed) : this(token, BuildMessage(token, allowed))
	{
	}
}
