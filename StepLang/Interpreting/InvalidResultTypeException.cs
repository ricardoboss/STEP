namespace StepLang.Interpreting;

public class InvalidResultTypeException : InterpreterException
{
    public InvalidResultTypeException(string expected, string got) : base($"Invalid result type: expected {expected}, got {got}")
    {
    }
}