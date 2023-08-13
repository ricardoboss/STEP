using System.Text;

namespace StepLang.Formatters;

public class FileEncodingFixer : IFileFixer
{
    private static readonly Encoding DefaultEncoding = Encoding.UTF8;

    public string Name => "FileEncodingFixer";

    public async Task<FileFixResult> FixAsync(FileInfo input, CancellationToken cancellationToken = default)
    {
        await using var stream = new FileStream(input.FullName, FileMode.Open, FileAccess.Read);
        var encoding = GetEncoding(stream);

        if (Equals(encoding, DefaultEncoding))
            return new(true, null, input);

        var tempFile = Path.GetTempFileName();
        await using var tempWriter = new StreamWriter(tempFile, false, DefaultEncoding);
        using var fileReader = new StreamReader(stream, encoding);
        await tempWriter.WriteAsync(await fileReader.ReadToEndAsync(cancellationToken));
        await tempWriter.FlushAsync();

        return new(true, null, new(tempFile));
    }

    private static Encoding GetEncoding(Stream stream)
    {
        using var reader = new StreamReader(stream, DefaultEncoding, true);

        // Detect byte order mark if any - otherwise assume default
        reader.Peek();

        return reader.CurrentEncoding;
    }
}