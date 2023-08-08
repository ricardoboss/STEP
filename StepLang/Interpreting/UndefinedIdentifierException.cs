using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public class UndefinedIdentifierException : InterpreterException
{
    public UndefinedIdentifierException(Token identifierToken) : base(identifierToken,$"Variable '{identifierToken.Value}' was not declared", $"You used a variable with the name '{identifierToken.Value}' but it was not declared. Use a declaration statement to declare it: `number {identifierToken.Value} = 0`")
    {
    }
}