using HILFE.Interpreting;

namespace HILFE;

public class InvalidScopeException : InterpreterException
{
    /// <inheritdoc />
    public InvalidScopeException(string message) : base(message)
    {
    }
}