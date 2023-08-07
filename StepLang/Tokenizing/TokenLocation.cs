namespace StepLang.Tokenizing;

public record TokenLocation(FileSystemInfo File, int Line, int Column)
{
    /// <inheritdoc />
    public override string ToString()
    {
        return $"{File.FullName}:{Line}:{Column}";
    }
}
