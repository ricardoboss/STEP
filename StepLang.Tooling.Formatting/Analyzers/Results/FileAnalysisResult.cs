namespace StepLang.Tooling.Formatting.Analyzers.Results;

/// <summary>
/// Represents the result of analyzing a file.
/// </summary>
public record FileAnalysisResult : AnalysisResult
{
    /// <summary>
    /// Creates a new instance of <see cref="FileAnalysisResult"/> with the fix provided in <paramref name="fixedFile"/>.
    /// </summary>
    /// <param name="fixedFile">The file with the fix.</param>
    /// <returns>A new instance of <see cref="FileAnalysisResult"/> with the fix provided in <paramref name="fixedFile"/>.</returns>
    public static FileAnalysisResult FixedAt(FileInfo fixedFile) => new(true, fixedFile);

    /// <summary>
    /// Creates a new instance of <see cref="FileAnalysisResult"/> with no fix.
    /// </summary>
    /// <param name="originalFile">The original file.</param>
    /// <returns>A new instance of <see cref="FileAnalysisResult"/> with no fix.</returns>
    public static FileAnalysisResult RetainOriginal(FileInfo originalFile) => new(false, originalFile);

    private FileAnalysisResult(bool FixRequired, FileInfo FixedFile) : base(FixRequired)
    {
        this.FixedFile = FixedFile;
    }

    /// <summary>
    /// The file with any fixes applied.
    /// </summary>
    public FileInfo FixedFile { get; }
}