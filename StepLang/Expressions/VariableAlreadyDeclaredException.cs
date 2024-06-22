using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Expressions;

/// <summary>
/// An exception thrown when a variable is declared twice.
/// </summary>
public class VariableAlreadyDeclaredException : IncompatibleTypesException
{
    /// <summary>
    /// Creates a new <see cref="VariableAlreadyDeclaredException"/> with the given identifier token.
    /// </summary>
    /// <param name="identifierToken">The token of the identifier that was already declared.</param>
    public VariableAlreadyDeclaredException(Token identifierToken) : base(5, identifierToken.Location, $"Variable {identifierToken.Value} is already declared.", "Make sure you are not declaring the same variable twice.")
    {
    }
}