using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Mutating;

public class DoRemoveFunction : NativeFunction
{
    public const string Identifier = "doRemove";

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 2);

        var (listExpression, elementExpression) = (arguments[0], arguments[1]);

        var list = await listExpression.EvaluateAsync(interpreter, r => r.ExpectList().Value, cancellationToken);
        var element = await elementExpression.EvaluateAsync(interpreter, cancellationToken);

        list.Remove(element);

        return VoidResult.Instance;
    }

    protected override string DebugParamsString => "list subject, any value";
}