using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public class UndefinedIdentifierException : InterpreterException
{
    public UndefinedIdentifierException(Token identifierToken) : base(identifierToken,$"Variable '{identifierToken.Value}' was not declared")
    {
    }
}