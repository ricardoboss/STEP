using StepLang.Tooling.Formatting.Analyzers.Results;

namespace StepLang.Tooling.Formatting.Analyzers;

public interface IFileAnalyzer : IAnalyzer
{
    /// <summary>
    /// Applies a fix to the given file. The fix should NOT override the file, but instead return a new file.
    /// </summary>
    /// <param name="input">The file to fix.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A result with a fixed file.</returns>
    public Task<FileAnalysisResult> AnalyzeAsync(FileInfo input, CancellationToken cancellationToken = default);
}