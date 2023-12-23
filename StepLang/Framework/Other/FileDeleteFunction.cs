using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Other;

/// <summary>
/// Deletes a file.
/// </summary>
public class FileDeleteFunction : GenericFunction<StringResult>
{
    /// <summary>
    /// The identifier of the <see cref="FileDeleteFunction"/>.
    /// </summary>
    public const string Identifier = "fileDelete";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyString, "path"),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyBool;

    /// <inheritdoc />
    protected override BoolResult Invoke(TokenLocation callLocation, Interpreter interpreter, StringResult argument1)
    {
        var path = argument1.Value;
        var info = callLocation.GetFileInfoFromPath(path);

        try
        {
            File.Delete(info.FullName);
        }
        catch (IOException)
        {
            return false;
        }

        return true;
    }
}