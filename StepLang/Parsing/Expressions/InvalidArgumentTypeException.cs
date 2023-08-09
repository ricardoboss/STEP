using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Expressions;

internal sealed class InvalidArgumentTypeException : IncompatibleTypesException
{
    public InvalidArgumentTypeException(Token parameterTypeToken, ExpressionResult argument) : base(parameterTypeToken.Location, $"Invalid argument type. Expected {parameterTypeToken.Value}, but got {argument.ResultType}", "Make sure you're passing the correct type of argument to the function.")
    {
    }
}