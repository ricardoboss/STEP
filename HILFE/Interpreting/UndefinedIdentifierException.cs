namespace HILFE.Interpreting;

public class UndefinedIdentifierException : InterpreterException
{
    public UndefinedIdentifierException()
    {
    }

    public UndefinedIdentifierException(string name) : base("Undefined variable: " + name)
    {
    }

    public UndefinedIdentifierException(string message, Exception innerException) : base(message, innerException)
    {
    }
}