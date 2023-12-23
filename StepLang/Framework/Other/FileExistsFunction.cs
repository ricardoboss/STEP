using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Other;

/// <summary>
/// Checks if a file exists.
/// </summary>
public class FileExistsFunction : GenericFunction<StringResult>
{
    /// <summary>
    /// The identifier of the <see cref="FileExistsFunction"/>.
    /// </summary>
    public const string Identifier = "fileExists";

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

        return info.Exists;
    }
}