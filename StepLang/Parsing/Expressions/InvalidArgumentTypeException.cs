using System.Globalization;
using System.Text;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Parsing.Expressions;

internal sealed class InvalidArgumentTypeException : IncompatibleTypesException
{
    private static string BuildMessage(IReadOnlyList<ResultType> acceptedTypes, ResultType gotType)
    {
        var sb = new StringBuilder("Invalid argument type. Expected ");

        sb.Append(acceptedTypes.Count switch
        {
            0 => "no arguments",
            1 => $"{acceptedTypes[0].ToTypeName()}",
            _ => $"{string.Join(", ", acceptedTypes.Take(acceptedTypes.Count - 1).Select(t => t.ToTypeName()))} or {acceptedTypes[^1].ToTypeName()}",
        });

        sb.Append(CultureInfo.InvariantCulture, $", but got {gotType.ToTypeName()}");

        return sb.ToString();
    }

    public InvalidArgumentTypeException(Token parameterTypeToken, ExpressionResult argument) : this(parameterTypeToken.Location, new[] { ValueTypeExtensions.FromTypeName(parameterTypeToken.Value) }, argument)
    {
    }

    public InvalidArgumentTypeException(TokenLocation? location, IReadOnlyList<ResultType> acceptedParameterTypes, ExpressionResult argument) : base(3, location, BuildMessage(acceptedParameterTypes, argument.ResultType), "Make sure you're passing the correct type of argument to the function.")
    {
    }
}