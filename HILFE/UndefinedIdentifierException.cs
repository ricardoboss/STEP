namespace HILFE;

public class UndefinedIdentifierException : InterpreterException
{
    public readonly string Name;

    public UndefinedIdentifierException(string name) : base("Undefined variable: " + name)
    {
        Name = name;
    }
}