using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Other;

public class FileDeleteFunction : NativeFunction
{
    public const string Identifier = "fileDelete";

    protected override IEnumerable<NativeParameter> NativeParameters => new NativeParameter[] { (new[] { ResultType.Str }, "path") };

    /// <inheritdoc />
    public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
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