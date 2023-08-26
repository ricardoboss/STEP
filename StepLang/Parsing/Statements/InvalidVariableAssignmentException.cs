using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Statements;

public class InvalidVariableAssignmentException : IncompatibleTypesException
{
    public InvalidVariableAssignmentException(Token identifierToken, IncompatibleVariableTypeException inner) : base(1, identifierToken.Location, inner.Message, inner.HelpText, inner)
    {
    }
}