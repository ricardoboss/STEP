using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Expressions;

/// <summary>
/// An exception type for invalid return types.
/// </summary>
public class InvalidReturnTypeException : InterpreterException
{
    /// <summary>
    /// Creates a new <see cref="InvalidReturnTypeException"/>.
    /// </summary>
    /// <param name="location">The location where the value was returned.</param>
    /// <param name="got">The result returned by the statement.</param>
    /// <param name="allowed">The types of results that are allowed to be returned.</param>
    public InvalidReturnTypeException(TokenLocation location, ResultType got, IEnumerable<ResultType> allowed) : base(6, location, $"Invalid return type. Got {got.ToTypeName()}, expected one of {string.Join(", ", allowed.Select(t => t.ToTypeName()))}", "Make sure the function returns a value with the correct type.")
    {
    }
}