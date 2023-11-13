using System.Text;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Framework.Other;

public class FileWriteFunction : GenericFunction<StringResult, StringResult, BoolResult>
{
    public const string Identifier = "fileWrite";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyString, "path"),
        new(OnlyString, "content"),
        new(OnlyBool, "append", DefaultValue: LiteralExpressionNode.FromBoolean(false)),
    };

    protected override BoolResult Invoke(TokenLocation tokenLocation, Interpreter interpreter, StringResult argument1, StringResult argument2, BoolResult argument3)
    {
        var path = argument1.Value;
        var content = argument2.Value;
        var append = argument3.Value;

        try
        {
            if (append)
                File.AppendAllText(path, content, Encoding.ASCII);
            else
            {
                var directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directory))
                    Directory.CreateDirectory(directory);

                File.WriteAllText(path, content, Encoding.ASCII);
            }
        }
        catch (Exception e) when (e is IOException or SystemException)
        {
            if (interpreter.DebugOut is { } debugOut)
                debugOut.WriteLine($"Failed to write to file '{path}': {e.Message}");

            return false;
        }

        return true;
    }
}