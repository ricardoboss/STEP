namespace StepLang.Interpreting;

public class ListIndexOutOfRangeException : InterpreterException
{
    public ListIndexOutOfRangeException(double index, int count) : base($"Index {index} is out of range for list with {count} elements")
    {
    }
}