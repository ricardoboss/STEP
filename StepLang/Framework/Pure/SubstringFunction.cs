using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class SubstringFunction : GenericFunction<StringResult, NumberResult, NumberResult>
{
    public const string Identifier = "substring";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyString, "subject"),
        new(OnlyNumber, "start"),
        new(OnlyNumber, "length"),
    };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyString;

    protected override StringResult Invoke(TokenLocation callLocation, Interpreter interpreter, StringResult argument1, NumberResult argument2, NumberResult argument3)
    {
        return argument1.Value.GraphemeSubstring(argument2, argument3);
    }
}