using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Statements;

public class VariableAlreadyDeclaredException : IncompatibleTypesException
{
    public VariableAlreadyDeclaredException(Token identifierToken) : base(5, identifierToken.Location, $"Variable {identifierToken.Value} is already declared.", "Make sure you are not declaring the same variable twice.")
    {
    }
}