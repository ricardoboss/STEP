using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Expressions;

public class InvalidIndexOperatorException : InterpreterException
{
    public InvalidIndexOperatorException(TokenLocation? location, string index, string valueType, string operation) : base(location, $"Invalid index expression: Cannot {operation} index {index} of a value of type {valueType}", "Make sure you're accessing an index of a value that supports indexing (like lists or maps).")
    {
    }
}