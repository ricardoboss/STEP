namespace HILFE.Interpreting;

public class InvalidScopeException : InterpreterException
{
    /// <inheritdoc />
    public InvalidScopeException(string message) : base(message)
    {
    }
}