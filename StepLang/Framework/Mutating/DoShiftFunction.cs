using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Mutating;

public class DoShiftFunction : NativeFunction
{
    public const string Identifier = "doShift";

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 1);

        var list = await arguments.Single().EvaluateAsync(interpreter, r => r.ExpectList().Value, cancellationToken);
        if (list.Count == 0)
            return NullResult.Instance;

        var value = list[0];
        list.RemoveAt(0);

        return value;
    }

    protected override string DebugParamsString => "list value";
}