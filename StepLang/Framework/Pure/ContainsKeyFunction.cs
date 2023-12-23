using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// Returns whether the given map contains the given key.
/// </summary>
public class ContainsKeyFunction : GenericFunction<MapResult, StringResult>
{
    /// <summary>
    /// The identifier of the <see cref="ContainsKeyFunction"/> function.
    /// </summary>
    public const string Identifier = "containsKey";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[] {
        new(OnlyMap, "subject"),
        new(OnlyString, "key"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyBool;

    /// <inheritdoc />
    protected override BoolResult Invoke(TokenLocation callLocation, Interpreter interpreter, MapResult argument1, StringResult argument2)
    {
        return argument1.Value.ContainsKey(argument2.Value);
    }
}
