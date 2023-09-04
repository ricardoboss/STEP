using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Conversion;

public class ToValuesFunction : NativeFunction
{
    public const string Identifier = "toValues";

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 1);

        var map = await arguments.Single().EvaluateAsync(interpreter, r => r.ExpectMap().Value, cancellationToken);
        var values = map.Values.ToList();

        return new ListResult(values);
    }

    protected override string DebugParamsString => "map subject";
}