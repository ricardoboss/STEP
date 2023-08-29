using System.Text;
using StepLang.Interpreting;
using StepLang.Parsing.Expressions;

namespace StepLang.Framework.Other;

public class FileWriteFunction : NativeFunction
{
    public const string Identifier = "fileWrite";

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
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
                await File.WriteAllTextAsync(path, content, Encoding.ASCII, cancellationToken);
        }
        catch (IOException)
        {
            return BoolResult.False;
        }

        return BoolResult.True;
    }

    /// <inheritdoc />
    protected override string DebugParamsString => "string path, string content, append = false";
}