using StepLang.Expressions.Results;
using StepLang.Framework.Conversion;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class PrintFunction : NativeFunction
{
    public const string Identifier = "print";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[] { new(Enum.GetValues<ResultType>(), "...values") };

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

    protected virtual void Print(TextWriter output, string value)
        => output.Write(value.AsMemory());
}