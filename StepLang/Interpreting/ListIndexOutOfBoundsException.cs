using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public class ListIndexOutOfBoundsException : InterpreterException
{
    public ListIndexOutOfBoundsException(double index, int count) : base((TokenLocation?)null, $"Index {index} is out of range for list with {count} elements", "Lists can only be indexed with numbers. The minimum index is 0 and the maximum index is the number of elements in the list minus 1. Make sure your index is within the bounds of the list.")
    {
    }
}