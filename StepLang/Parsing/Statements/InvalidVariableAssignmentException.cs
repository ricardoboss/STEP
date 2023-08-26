using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

public class InvalidVariableAssignmentException : IncompatibleTypesException
{
    public InvalidVariableAssignmentException(Token identifierToken, StepLangException inner) : base(6, identifierToken.Location, inner.Message, inner.HelpText, inner)
    {
    }
}