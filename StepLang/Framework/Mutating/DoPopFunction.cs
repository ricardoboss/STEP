using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Mutating;

public class DoPopFunction : NativeFunction
{
    public const string Identifier = "doPop";

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 1);

        var list = await arguments.Single().EvaluateAsync(interpreter, r => r.ExpectList().Value, cancellationToken);
        if (list.Count == 0)
            return NullResult.Instance;

        var value = list.Last();

        list.RemoveAt(list.Count - 1);

        return value;
    }

    protected override string DebugParamsString => "list subject";
}