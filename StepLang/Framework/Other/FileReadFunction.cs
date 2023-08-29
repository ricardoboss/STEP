using System.Text;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Other;

public class FileReadFunction : NativeFunction
{
    public const string Identifier = "fileRead";

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 1);

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

    /// <inheritdoc />
    protected override string DebugParamsString => "string path";
}