using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Web;

public class StringResponseFunction : NativeFunction
{
    public const string Identifier = "stringResponse";

    /// <inheritdoc />
    public override IEnumerable<(ResultType [] types, string identifier)> Parameters => new[] { (new[] { ResultType.Str }, "text"), (new[] { ResultType.Number }, "status") };

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 1, 2);

        var text = await arguments[0].EvaluateAsync(interpreter, r => r.ExpectString().Value, cancellationToken);

        var response = new Dictionary<string, ExpressionResult>
        {
            ["body"] = new StringResult(text),
        };

        if (arguments.Count == 2)
        {
            var status = await arguments[1].EvaluateAsync(interpreter, r => r.ExpectInteger().RoundedIntValue, cancellationToken);

            response["status"] = new NumberResult(status);
        }

        return new MapResult(response);
    }
}