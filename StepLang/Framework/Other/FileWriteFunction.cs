using System.Text;
using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Other;

public class FileWriteFunction : NativeFunction
{
    public const string Identifier = "fileWrite";

    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.Str }, "path"), (new[] { ResultType.Str }, "content"), (new[] { ResultType.Bool }, "append") };

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 2, 3);

        var path = await arguments[0].EvaluateAsync(interpreter, r => r.ExpectString().Value, cancellationToken);
        var content = await arguments[1].EvaluateAsync(interpreter, r => r.ExpectString().Value, cancellationToken);

        var append = false;
        if (arguments.Count >= 3)
            append = await arguments[2].EvaluateAsync(interpreter, r => r.ExpectBool().Value, cancellationToken);

        try
        {
            if (append)
                await File.AppendAllTextAsync(path, content, Encoding.ASCII, cancellationToken);
            else
            {
                var directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directory))
                    Directory.CreateDirectory(directory);

                await File.WriteAllTextAsync(path, content, Encoding.ASCII, cancellationToken);
            }
        }
        catch (Exception e) when (e is IOException or SystemException)
        {
            return BoolResult.False;
        }

        return BoolResult.True;
    }
}