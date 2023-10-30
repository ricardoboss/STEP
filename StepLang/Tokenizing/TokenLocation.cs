namespace StepLang.Tokenizing;

/// <summary>
/// Represents the location of a token in a file.
/// </summary>
/// <param name="File">The file containing the token.</param>
/// <param name="Line">The line number of the token.</param>
/// <param name="Column">The column number of the token.</param>
/// <param name="Length">The length of the token.</param>
public record TokenLocation(FileSystemInfo? File = null, int Line = 1, int Column = 1, int Length = 0)
{
    /// <inheritdoc />
    public override string ToString()
    {
        if (File is not null)
            return $"{File.FullName}:{Line}:{Column}";

        return $"{Line}:{Column}";
    }
}
