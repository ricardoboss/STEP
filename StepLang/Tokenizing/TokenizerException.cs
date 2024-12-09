namespace StepLang.Tokenizing;

public abstract class TokenizerException(
	int errorCode,
	TokenLocation? location,
	string message,
	string helpText,
	Exception? inner = null)
	: StepLangException($"TOK{errorCode:000}", location, message, helpText, inner);
