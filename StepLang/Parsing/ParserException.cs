using StepLang.Tokenizing;

namespace StepLang.Parsing;

public abstract class ParserException(
	int errorCode,
	TokenLocation? location,
	string message,
	string helpText,
	Exception? inner = null)
	: StepLangException($"PAR{errorCode:000}", location, message, helpText, inner)
{
	protected ParserException(int errorCode, Token? token, string message, string helpText, Exception? inner = null) :
		this(errorCode, token?.Location, message, helpText, inner)
	{
	}
}
