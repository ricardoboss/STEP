using System.Diagnostics.CodeAnalysis;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class UndefinedIdentifierException : InterpreterException
{
    public UndefinedIdentifierException(Token identifierToken) : base(identifierToken,$"Undefined variable: {identifierToken.Value}")
    {
    }
}