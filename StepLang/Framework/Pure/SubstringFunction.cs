using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// Returns a substring of the given string.
/// </summary>
public class SubstringFunction : GenericFunction<StringResult, NumberResult, ExpressionResult>
{
    /// <summary>
    /// The identifier of the <see cref="SubstringFunction"/> function.
    /// </summary>
    public const string Identifier = "substring";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyString, "subject"),
        new(OnlyNumber, "start"),
        new(NullableNumber, "length", LiteralExpressionNode.Null),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyString;

    /// <inheritdoc />
    protected override StringResult Invoke(TokenLocation callLocation, Interpreter interpreter, StringResult argument1, NumberResult argument2, ExpressionResult argument3)
    {
        int? length;
        if (argument3 is NumberResult number)
            length = number;
        else if (argument3 is NullResult)
            length = null;
        else
            throw new InvalidArgumentTypeException(callLocation, [ResultType.Number, ResultType.Null], argument3);

        return argument1.Value.GraphemeSubstring(argument2, length);
    }
}