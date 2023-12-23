using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// Reads a single character from standard input and returns it.
/// </summary>
public class ReadFunction : GenericFunction
{
    /// <summary>
    /// The identifier of the function.
    /// </summary>
    public const string Identifier = "read";

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = NullableString;

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter)
    {
        if (interpreter.StdIn is not { } stdIn)
            return NullResult.Instance;

        var character = stdIn.Read();
        if (character < 0)
            return NullResult.Instance;

        return new StringResult(char.ConvertFromUtf32(character));
    }
}