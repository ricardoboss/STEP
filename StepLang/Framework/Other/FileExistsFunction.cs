using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Framework.Other;

public class FileExistsFunction : GenericFunction<StringResult>
{
    public const string Identifier = "fileExists";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyString, "path"),
    };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyBool;

    protected override BoolResult Invoke(Interpreter interpreter, StringResult argument1)
    {
        var path = argument1.Value;

        return File.Exists(path);
    }
}