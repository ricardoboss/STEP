using StepLang.Expressions.Results;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Expressions;

/// <summary>
/// Thrown when an invalid index operator is used.
/// </summary>
public class InvalidIndexOperatorException : ParserException
{
    /// <summary>
    /// Creates a new InvalidIndexOperatorException.
    /// </summary>
    /// <param name="location">The location of the index access.</param>
    /// <param name="index">The index that was accessed.</param>
    /// <param name="resultType">The type of the value that was accessed.</param>
    /// <param name="operation">The operation that was attempted.</param>
    public InvalidIndexOperatorException(TokenLocation location, string index, ResultType resultType, string operation) : base(4, location, $"Invalid index expression: Cannot {operation} index {index} of a value of type {resultType.ToTypeName()}", "Make sure you're accessing an index of a value that supports indexing (like lists or maps).")
    {
    }
}