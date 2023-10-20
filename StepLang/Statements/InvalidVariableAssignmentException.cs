using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Statements;

public class InvalidVariableAssignmentException : IncompatibleTypesException
{
    public InvalidVariableAssignmentException(Token identifierToken, IncompatibleVariableTypeException inner) : base(1, identifierToken.Location, inner.Message, inner.HelpText, inner)
    {
    }

    public InvalidVariableAssignmentException(Token identifierToken, NonNullableVariableAssignmentException inner) : base(1, identifierToken.Location, inner.Message, inner.HelpText, inner)
    {
    }
}