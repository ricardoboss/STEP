using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// Reads a single line from standard input and returns it.
/// </summary>
public class ReadlineFunction : GenericFunction
{
    /// <summary>
    /// The identifier of the <see cref="ReadlineFunction"/> function.
    /// </summary>
    public const string Identifier = "readline";

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = NullableString;

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter)
    {
        if (interpreter.StdIn is not { } stdIn)
            return NullResult.Instance;

        var line = stdIn.ReadLine();

        return line is null ? NullResult.Instance : new StringResult(line);
    }
}