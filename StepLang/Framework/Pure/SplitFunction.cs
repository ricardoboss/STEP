using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class SplitFunction : GenericFunction<StringResult, StringResult>
{
    public const string Identifier = "split";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyString, "source"),
        new(OnlyString, "separator", LiteralExpressionNode.FromString("")),
    };

    protected override ListResult Invoke(TokenLocation callLocation, Interpreter interpreter, StringResult argument1, StringResult argument2)
    {
        var source = argument1.Value;
        var separator = argument2.Value;

        var parts = source
            .GraphemeSplit(separator)
            .Select(StringResult.FromString)
            .Cast<ExpressionResult>()
            .ToList();

        return new(parts);
    }
}