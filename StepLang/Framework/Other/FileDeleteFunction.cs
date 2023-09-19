using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Other;

public class FileDeleteFunction : NativeFunction
{
    public const string Identifier = "fileDelete";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.Str }, "path") };

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var path = await arguments.Single().EvaluateAsync(interpreter, r => r.ExpectString().Value, cancellationToken);

        try
        {
            File.Delete(path);
        }
        catch (IOException)
        {
            return BoolResult.False;
        }

        return BoolResult.True;
    }
}