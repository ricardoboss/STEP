using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Reflection;

public class DebugPrintFunction : NativeFunction
{
    public const string Identifier = "debugPrint";

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 1);

        if (interpreter.StdOut is { } stdOut)
            await stdOut.WriteLineAsync(arguments.Single().ToString());

        return VoidResult.Instance;
    }

    protected override string DebugParamsString => "any value";
}