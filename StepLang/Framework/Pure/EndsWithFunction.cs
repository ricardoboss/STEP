using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// Checks whether a string ends with a given suffix.
/// </summary>
public class EndsWithFunction : GenericFunction<StringResult, StringResult>
{
    /// <summary>
    /// The identifier of the <see cref="EndsWithFunction"/> function.
    /// </summary>
    public const string Identifier = "endsWith";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyString, "subject"),
        new(OnlyString, "suffix"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyBool;

    /// <inheritdoc />
    protected override BoolResult Invoke(TokenLocation callLocation, Interpreter interpreter, StringResult argument1, StringResult argument2)
    {
        return argument1.Value.GraphemeEndsWith(argument2.Value);
    }
}