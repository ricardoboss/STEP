using StepLang.Expressions.Results;
using StepLang.Framework.Conversion;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

/// <summary>
/// A function that prints to STDOUT.
/// </summary>
public class PrintFunction : NativeFunction
{
    /// <summary>
    /// The identifier of the <see cref="PrintFunction"/> function.
    /// </summary>
    public const string Identifier = "print";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[] { new(Enum.GetValues<ResultType>(), "...values") };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes => new[] { ResultType.Void };

    /// <inheritdoc />
    public override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        IReadOnlyList<ExpressionNode> arguments)
    {
        if (interpreter.StdOut is not { } stdOut)
            return VoidResult.Instance;

        var stringArgs = arguments
            .EvaluateUsing(interpreter)
            .Select(ToStringFunction.Render)
            .ToList();

        Print(stdOut, string.Join("", stringArgs));
        stdOut.Flush();

        return VoidResult.Instance;
    }

    /// <summary>
    /// Prints the given value to the given output.
    /// </summary>
    /// <param name="output">The output to print to.</param>
    /// <param name="value">The value to print.</param>
    protected virtual void Print(TextWriter output, string value)
        => output.Write(value.AsMemory());
}