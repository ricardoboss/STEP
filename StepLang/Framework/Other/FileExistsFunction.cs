using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Other;

public class FileExistsFunction : NativeFunction
{
    public const string Identifier = "fileExists";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[] { (new[] { ResultType.Str }, "path") };

    /// <inheritdoc />
    public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        CheckArgumentCount(arguments);

        var path = await arguments.Single().EvaluateAsync(interpreter, r => r.ExpectString().Value, cancellationToken);

        return new BoolResult(File.Exists(path));
    }
}