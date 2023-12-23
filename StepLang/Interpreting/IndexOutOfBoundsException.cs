using StepLang.Tokenizing;

namespace StepLang.Interpreting;

/// <summary>
/// Thrown when a list is indexed with an invalid index.
/// </summary>
public class IndexOutOfBoundsException : InterpreterException
{
    private static string GetMessage(int index, int count)
    {
        return count switch
        {
            < 0 => throw new ArgumentOutOfRangeException(nameof(count)),
            0 => "The list is empty, so there are no valid indexes.",
            > 0 => $"Index {index} is out of range (min 0, max {count - 1})",
        };
    }

    /// <summary>
    /// Creates a new <see cref="IndexOutOfBoundsException"/> with the given location, index, and count.
    /// </summary>
    /// <param name="location">The location where the index expression was found.</param>
    /// <param name="index">The index that was used.</param>
    /// <param name="count">The number of elements in the list.</param>
    public IndexOutOfBoundsException(TokenLocation location, int index, int count) : base(5, location, GetMessage(index, count), "Lists can only be indexed with numbers. The minimum index is 0 and the maximum index is the number of elements in the list minus 1. Make sure your index is within the bounds of the list and that the list is not empty.")
    {
    }
}