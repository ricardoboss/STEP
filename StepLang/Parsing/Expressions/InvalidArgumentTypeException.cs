using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Expressions;

internal sealed class InvalidArgumentTypeException : IncompatibleTypesException
{
    public InvalidArgumentTypeException(Token parameterTypeToken, ExpressionResult argument) : base(4, parameterTypeToken.Location, $"Invalid argument type. Expected {parameterTypeToken.Value}, but got {argument.ResultType.ToTypeName()}", "Make sure you're passing the correct type of argument to the function.")
    {
    }
}