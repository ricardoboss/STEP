using StepLang.Expressions.Results;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Expressions;

public class InvalidIndexOperatorException : ParserException
{
    public InvalidIndexOperatorException(TokenLocation location, string index, ResultType resultType, string operation) : base(4, location, $"Invalid index expression: Cannot {operation} index {index} of a value of type {resultType.ToTypeName()}", "Make sure you're accessing an index of a value that supports indexing (like lists or maps).")
    {
    }
}