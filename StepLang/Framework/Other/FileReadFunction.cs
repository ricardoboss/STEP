using System.Text;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Other;

public class FileReadFunction : GenericFunction<StringResult>
{
    public const string Identifier = "fileRead";

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyString, "path"),
    };

    protected override IEnumerable<ResultType> ReturnTypes { get; } = NullableString;

    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        StringResult argument1)
    {
        var path = argument1.Value;

        if (!File.Exists(path))
            return NullResult.Instance;

        try
        {
            var contents = File.ReadAllText(path, Encoding.ASCII);

            return new StringResult(contents);
        }
        catch (IOException)
        {
            return NullResult.Instance;
        }
    }
}