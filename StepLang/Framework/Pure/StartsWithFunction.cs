using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// Returns true if the string starts with the given prefix.
/// </summary>
public class StartsWithFunction : GenericFunction<StringResult, StringResult>
{
    /// <summary>
    /// The identifier of the <see cref="StartsWithFunction"/> function.
    /// </summary>
    public const string Identifier = "startsWith";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyString, "subject"),
        new(OnlyString, "prefix"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyBool;

    /// <inheritdoc />
    protected override BoolResult Invoke(TokenLocation callLocation, Interpreter interpreter, StringResult argument1, StringResult argument2)
    {
        return argument1.Value.GraphemeStartsWith(argument2.Value);
    }
}