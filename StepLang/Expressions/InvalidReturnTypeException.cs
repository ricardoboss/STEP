using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Expressions;

public class InvalidReturnTypeException : InterpreterException
{
    public InvalidReturnTypeException(TokenLocation? location, ResultType got, IEnumerable<ResultType> allowed) : base(6, location, $"Invalid return type. Got {got.ToTypeName()}, expected one of {string.Join(", ", allowed.Select(t => t.ToTypeName()))}", "Make sure the function returns a value with the correct type.")
    {
    }
}