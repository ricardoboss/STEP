using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public abstract class IncompatibleTypesException(
	int errorCode,
	TokenLocation location,
	string message,
	string helpText,
	Exception? inner = null)
	: InterpreterException($"TYP{errorCode:000}", location, message, helpText, inner);
