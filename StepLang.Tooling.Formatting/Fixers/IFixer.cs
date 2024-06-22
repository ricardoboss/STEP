using StepLang.Tooling.Formatting.Analyzers;

namespace StepLang.Tooling.Formatting.Fixers;

/// <summary>
/// A fixer that applies analyzer fixes to files.
/// </summary>
public interface IFixer
{
    /// <summary>
    /// Fired before a fixer applies a fix.
    /// </summary>
    public event EventHandler<BeforeFixerRanEventArgs>? OnCheck;

    /// <summary>
    /// Fired after a fixer applied a fix.
    /// </summary>
    public event EventHandler<AfterFixerRanEventArgs>? OnFixed;

    /// <summary>
    /// Whether or not to throw an exception when a fixer fails.
    /// </summary>
    public bool ThrowOnFailure { get; init; }

    /// <summary>
    /// Apply an <see cref="IAnalyzer"/> to a file.
    /// </summary>
    /// <param name="analyzer">The analyzer to apply.</param>
    /// <param name="file">The file to apply the analyzer to.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="FixerResult"/> containing the results of the fix.</returns>
    public Task<FixerResult> FixAsync(IAnalyzer analyzer, FileInfo file, CancellationToken cancellationToken = default);

    /// <summary>
    /// Apply a list of <see cref="IAnalyzer"/>s to a file.
    /// </summary>
    /// <param name="analyzers">The analyzers to apply.</param>
    /// <param name="file">The file to apply the analyzers to.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="FixerResult"/> containing the results of the fix.</returns>
    public Task<FixerResult> FixAsync(IEnumerable<IAnalyzer> analyzers, FileInfo file, CancellationToken cancellationToken = default);

    /// <summary>
    /// Apply an <see cref="IAnalyzer"/> to a directory.
    /// </summary>
    /// <param name="analyzer">The analyzer to apply.</param>
    /// <param name="directory">The directory to apply the analyzer to.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="FixerResult"/> containing the results of the fix.</returns>
    public Task<FixerResult> FixAsync(IAnalyzer analyzer, DirectoryInfo directory, CancellationToken cancellationToken = default);

    /// <summary>
    /// Apply a list of <see cref="IAnalyzer"/>s to a directory.
    /// </summary>
    /// <param name="analyzers">The analyzers to apply.</param>
    /// <param name="directory">The directory to apply the analyzers to.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="FixerResult"/> containing the results of the fix.</returns>
    public Task<FixerResult> FixAsync(IEnumerable<IAnalyzer> analyzers, DirectoryInfo directory, CancellationToken cancellationToken = default);

    /// <summary>
    ///Apply an <see cref="IAnalyzer"/> to a list of files.
    /// </summary>
    /// <param name="analyzer">The analyzer to apply.</param>
    /// <param name="files">The files to apply the analyzer to.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="FixerResult"/> containing the results of the fix.</returns>
    public Task<FixerResult> FixAsync(IAnalyzer analyzer, IEnumerable<FileInfo> files, CancellationToken cancellationToken = default);

    /// <summary>
    /// Apply a list of <see cref="IAnalyzer"/>s to a list of files.
    /// </summary>
    /// <param name="analyzers">The analyzers to apply.</param>
    /// <param name="files">The files to apply the analyzers to.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="FixerResult"/> containing the results of the fix.</returns>
    public Task<FixerResult> FixAsync(IEnumerable<IAnalyzer> analyzers, IEnumerable<FileInfo> files, CancellationToken cancellationToken = default);

    /// <summary>
    ///Apply an <see cref="IAnalyzer"/> to a list of directories.
    /// </summary>
    /// <param name="analyzer">The analyzer to apply.</param>
    /// <param name="directories">The directories to apply the analyzer to.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="FixerResult"/> containing the results of the fix.</returns>
    public Task<FixerResult> FixAsync(IAnalyzer analyzer, IEnumerable<DirectoryInfo> directories, CancellationToken cancellationToken = default);

    /// <summary>
    /// Apply a list of <see cref="IAnalyzer"/>s to a list of directories.
    /// </summary>
    /// <param name="analyzers">The analyzers to apply.</param>
    /// <param name="directories">The directories to apply the analyzers to.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="FixerResult"/> containing the results of the fix.</returns>
    public Task<FixerResult> FixAsync(IEnumerable<IAnalyzer> analyzers, IEnumerable<DirectoryInfo> directories, CancellationToken cancellationToken = default);
}