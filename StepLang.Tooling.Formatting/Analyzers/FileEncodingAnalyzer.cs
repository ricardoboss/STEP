using System.Text;
using StepLang.Tooling.Formatting.Analyzers.Results;

namespace StepLang.Tooling.Formatting.Analyzers;

/// <summary>
/// Check if the file is encoded in UTF-8 and provides a fix if it is not.
/// </summary>
public class FileEncodingAnalyzer : IFileAnalyzer
{
    private static readonly Encoding DefaultEncoding = Encoding.UTF8;

    /// <inheritdoc />
    public async Task<FileAnalysisResult> AnalyzeAsync(FileInfo original, CancellationToken cancellationToken = default)
    {
        await using var stream = new FileStream(original.FullName, FileMode.Open, FileAccess.Read);

        var usedEncoding = GetEncoding(stream, DefaultEncoding);
        if (Equals(usedEncoding, DefaultEncoding))
            return FileAnalysisResult.RetainOriginal(original);

        var tempFile = Path.GetTempFileName();

        await using var tempWriter = new StreamWriter(tempFile, false, DefaultEncoding);
        using var fileReader = new StreamReader(stream, usedEncoding);
        await tempWriter.WriteAsync(await fileReader.ReadToEndAsync(cancellationToken));
        await tempWriter.FlushAsync();

        return FileAnalysisResult.FixedAt(new(tempFile));
    }

    private static Encoding GetEncoding(Stream stream, Encoding fallback)
    {
        using var reader = new StreamReader(stream, fallback, true);

        // Detect byte order mark if any - otherwise assume default
        reader.Peek();

        return reader.CurrentEncoding;
    }
}