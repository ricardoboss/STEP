namespace StepLang.Interpreting;

public abstract class InterpreterException : Exception
{
    protected InterpreterException()
    {
    }

    protected InterpreterException(string message) : base(message)
    {
    }

    protected InterpreterException(string message, Exception innerException) : base(message, innerException)
    {
    }
}