using StepLang.Expressions.Results;
using StepLang.Framework.Conversion;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Pure;

public class PrintFunction : NativeFunction
{
    public const string Identifier = "print";

    protected override IEnumerable<(ResultType[] types, string identifier)> NativeParameters => new[] { (Enum.GetValues<ResultType>(), "...values") };

    public override IEnumerable<ResultType> ReturnTypes => new[] { ResultType.Void };

    /// <inheritdoc />
    public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        if (interpreter.StdOut is not { } stdOut)
            return VoidResult.Instance;

        var stringArgs = arguments
            .EvaluateUsing(interpreter)
            .Select(ToStringFunction.Render)
            .ToList();

        Print(stdOut, string.Join("", stringArgs));

        return VoidResult.Instance;
    }

    protected virtual void Print(TextWriter output, string value)
        => output.Write(value.AsMemory());
}