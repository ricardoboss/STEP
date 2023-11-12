namespace StepLang.Tokenizing;

public record TokenLocation(FileSystemInfo? File = null, int Line = 1, int Column = 1, int? Length = null)
{
    /// <inheritdoc />
    public override string ToString()
    {
        if (File is not null)
            return $"{File.FullName}:{Line}:{Column}";

        return $"{Line}:{Column}";
    }
}
