using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Expressions;

internal sealed class InvalidArgumentValueException(TokenLocation location, string message) : InterpreterException(7,
	location, message,
	"Make sure you're passing a value supported by the function.");
