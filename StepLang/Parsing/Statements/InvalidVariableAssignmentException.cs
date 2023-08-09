using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

public class InvalidVariableAssignmentException : IncompatibleTypesException
{
    public InvalidVariableAssignmentException(Token identifierToken, StepLangException inner) : base(identifierToken.Location, inner.Message, inner.HelpText ?? "Make sure the value you are assigning is of the same type as the variable you are assigning to.", inner)
    {
    }
}