using System.Text;
using StepLang.Tooling.Formatting.Fixers.Results;

namespace StepLang.Tooling.Formatting.Fixers;

public class FileEncodingFixer : IFileFixer
{
    public string Name => "FileEncodingFixer";

    public async Task<FileFixResult> FixAsync(FileInfo input, FixerConfiguration configuration, CancellationToken cancellationToken = default)
    {
        var expectedEncoding = configuration.ParsedEncoding;

        await using var stream = new FileStream(input.FullName, FileMode.Open, FileAccess.Read);

        var usedEncoding = GetEncoding(stream, expectedEncoding);
        if (Equals(usedEncoding, expectedEncoding))
            return new(false, input);

        var tempFile = Path.GetTempFileName();

        await using var tempWriter = new StreamWriter(tempFile, false, expectedEncoding);
        using var fileReader = new StreamReader(stream, usedEncoding);
        await tempWriter.WriteAsync(await fileReader.ReadToEndAsync(cancellationToken));
        await tempWriter.FlushAsync();

        return new(true, new(tempFile));
    }

    private static Encoding GetEncoding(Stream stream, Encoding fallback)
    {
        using var reader = new StreamReader(stream, fallback, true);

        // Detect byte order mark if any - otherwise assume default
        reader.Peek();

        return reader.CurrentEncoding;
    }
}