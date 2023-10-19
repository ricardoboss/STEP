using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Web;

public class FileResponseFunction : NativeFunction
{
    public const string Identifier = "fileResponse";

    /// <inheritdoc />
    public override IEnumerable<(ResultType [] types, string identifier)> Parameters => new[] { (new[] { ResultType.Str }, "file"), (new[] { ResultType.Number }, "status") };

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 1, 2);

        var file = await arguments[0].EvaluateAsync(interpreter, r => r.ExpectString().Value, cancellationToken);
        if (!File.Exists(file))
        {
            return new MapResult(new Dictionary<string, ExpressionResult>
            {
                ["status"] = new NumberResult(404),
            });
        }

        var fileContents = await File.ReadAllTextAsync(file, cancellationToken);

        var response = new Dictionary<string, ExpressionResult>
        {
            ["body"] = new StringResult(fileContents),
        };

        if (arguments.Count == 2)
        {
            var status = await arguments[1].EvaluateAsync(interpreter, r => r.ExpectInteger().RoundedIntValue, cancellationToken);

            response["status"] = new NumberResult(status);
        }

        return new MapResult(response);
    }
}