using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public class IndexOutOfBoundsException : InterpreterException
{
    public IndexOutOfBoundsException(double index, int count) : base(5, (TokenLocation?)null, $"Index {index} is out of range (min 0, max {count - 1})", "Lists can only be indexed with numbers. The minimum index is 0 and the maximum index is the number of elements in the list minus 1. Make sure your index is within the bounds of the list.")
    {
    }
}