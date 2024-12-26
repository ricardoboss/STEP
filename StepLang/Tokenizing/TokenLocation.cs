namespace StepLang.Tokenizing;

public record TokenLocation(Uri? DocumentUri = null, int Line = 1, int Column = 1, int Length = 0)
{
    /// <inheritdoc />
    public override string ToString()
    {
        if (File is {} file)
            return $"{file.Name}:{Line}:{Column}";

        if (DocumentUri != null)
            return $"{DocumentUri.ToString()}:{Line}:{Column}";

        return $"{Line}:{Column}";
    }

    public FileSystemInfo? File
    {
        get
        {
            if (DocumentUri is null)
                return null;

            return DocumentUri.IsFile ? new FileInfo(DocumentUri.AbsolutePath) : null;
        }
    }
}
