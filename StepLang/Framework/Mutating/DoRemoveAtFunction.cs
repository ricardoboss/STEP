using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Mutating;

public class DoRemoveAtFunction : NativeFunction
{
    public const string Identifier = "doRemoveAt";

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 2);

        var (listExpression, elementExpression) = (arguments[0], arguments[1]);

        var list = await listExpression.EvaluateAsync(interpreter, r => r.ExpectList().Value, cancellationToken);
        var index = await elementExpression.EvaluateAsync(interpreter, r => r.ExpectInteger().RoundedIntValue, cancellationToken);

        if (list.Count == 0 || index < 0 || index >= list.Count)
            throw new ListIndexOutOfBoundsException(index, list.Count);

        var element = list[index];
        list.RemoveAt(index);

        return element;
    }

    protected override string DebugParamsString => "list subject, any value";
}