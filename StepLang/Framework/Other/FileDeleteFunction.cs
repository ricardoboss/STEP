using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Other;

public class FileDeleteFunction : GenericFunction<StringResult>
{
    public const string Identifier = "fileDelete";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyString, "path"),
    };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyBool;

    protected override BoolResult Invoke(Interpreter interpreter, StringResult argument1)
    {
        var path = argument1.Value;

        try
        {
            File.Delete(path);
        }
        catch (IOException)
        {
            return false;
        }

        return true;
    }
}