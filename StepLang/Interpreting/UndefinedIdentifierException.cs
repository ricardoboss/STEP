using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public class UndefinedIdentifierException(Token identifierToken) : InterpreterException(1, identifierToken,
	$"Variable '{identifierToken.Value}' was not declared (used at {identifierToken.Location})",
	$"You used a variable with the name '{identifierToken.Value}' but it was not declared. Use a declaration statement to declare it: `number {identifierToken.Value} = 0`");
