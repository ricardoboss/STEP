using StepLang.Tokenizing;

namespace StepLang.Parsing.Expressions;

public class InvalidIndexOperatorException : ParserException
{
    public InvalidIndexOperatorException(TokenLocation? location, string index, ResultType resultType, string operation) : base(10, location, $"Invalid index expression: Cannot {operation} index {index} of a value of type {resultType.ToTypeName()}", "Make sure you're accessing an index of a value that supports indexing (like lists or maps).")
    {
    }
}