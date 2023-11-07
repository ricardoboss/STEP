using System.Text;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Other;

public class FileReadFunction : NativeFunction
{
    public const string Identifier = "fileRead";

    protected override IEnumerable<NativeParameter> NativeParameters => new NativeParameter[] { (new[] { ResultType.Str }, "path") };

    /// <inheritdoc />
    public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        CheckArgumentCount(arguments);

        var path = await arguments.Single().EvaluateAsync(interpreter, r => r.ExpectString().Value, cancellationToken);

        if (!File.Exists(path))
            return NullResult.Instance;

        try
        {
            var contents = await File.ReadAllTextAsync(path, Encoding.ASCII, cancellationToken);

            return new StringResult(contents);
        }
        catch (IOException)
        {
            return NullResult.Instance;
        }
    }
}