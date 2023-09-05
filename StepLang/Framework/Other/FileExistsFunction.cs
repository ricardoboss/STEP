using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Other;

public class FileExistsFunction : NativeFunction
{
    public const string Identifier = "fileExists";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.Str }, "path") };

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var path = await arguments.Single().EvaluateAsync(interpreter, r => r.ExpectString().Value, cancellationToken);

        return new BoolResult(File.Exists(path));
    }
}