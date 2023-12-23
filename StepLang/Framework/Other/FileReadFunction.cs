using System.Text;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Other;

/// <summary>
/// Reads a file.
/// </summary>
public class FileReadFunction : GenericFunction<StringResult>
{
    /// <summary>
    /// The identifier of the <see cref="FileReadFunction"/>.
    /// </summary>
    public const string Identifier = "fileRead";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyString, "path"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = NullableString;

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        StringResult argument1)
    {
        var path = argument1.Value;
        var info = callLocation.GetFileInfoFromPath(path);

        if (!info.Exists)
            return NullResult.Instance;

        try
        {
            var contents = File.ReadAllText(info.FullName, Encoding.ASCII);

            return new StringResult(contents);
        }
        catch (IOException)
        {
            return NullResult.Instance;
        }
    }
}