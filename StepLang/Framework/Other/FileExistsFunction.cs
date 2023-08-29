using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Other;

public class FileExistsFunction : NativeFunction
{
    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 1);

        var path = await arguments.Single().EvaluateAsync(interpreter, r => r.ExpectString().Value, cancellationToken);

        return new BoolResult(File.Exists(path));
    }

    /// <inheritdoc />
    protected override string DebugParamsString => "string path";
}