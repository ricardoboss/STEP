using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Expressions;

public class VariableAlreadyDeclaredException(Token identifierToken) : IncompatibleTypesException(5,
	identifierToken.Location,
	$"Variable {identifierToken.Value} is already declared.",
	"Make sure you are not declaring the same variable twice.");
