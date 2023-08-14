using StepLang.Tooling.Formatting.Fixers.Results;

namespace StepLang.Tooling.Formatting.Fixers;

public interface IFileFixer : IFixer
{
    /// <summary>
    /// Applies a fix to the given file. The fix should NOT override the file, but instead return a new file.
    /// </summary>
    /// <param name="input">The file to fix.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A result with a fixed file.</returns>
    public Task<FileFixResult> FixAsync(FileInfo input, CancellationToken cancellationToken = default);
}