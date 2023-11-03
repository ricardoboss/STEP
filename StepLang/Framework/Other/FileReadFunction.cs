using System.Text;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Other;

public class FileReadFunction : NativeFunction
{
    public const string Identifier = "fileRead";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.Str }, "path") };

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
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