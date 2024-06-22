using System.Diagnostics;
using StepLang.Tooling.Formatting.Analyzers;
using StepLang.Tooling.Formatting.Analyzers.Results;

namespace StepLang.Tooling.Formatting.Fixers;

/// <summary>
/// Base class for implementations of <see cref="IFixer"/>.
/// </summary>
public abstract class BaseFixer : IFixer
{
    private const string DefaultSearchPattern = "*.step";

    /// <inheritdoc/>
    public virtual bool ThrowOnFailure { get; init; } = true;

    /// <inheritdoc/>
    public Task<FixerResult> FixAsync(IAnalyzer analyzer, DirectoryInfo directory,
        CancellationToken cancellationToken = default) =>
        FixAsync(new[] { analyzer }, new[] { directory }, cancellationToken);

    /// <inheritdoc/>
    public Task<FixerResult> FixAsync(IEnumerable<IAnalyzer> analyzers, DirectoryInfo directory,
        CancellationToken cancellationToken = default) =>
        FixAsync(analyzers, new[] { directory }, cancellationToken);

    /// <inheritdoc/>
    public Task<FixerResult> FixAsync(IAnalyzer analyzer, IEnumerable<FileInfo> files,
        CancellationToken cancellationToken = default) =>
        FixAsync(new[] { analyzer }, files, cancellationToken);

    /// <inheritdoc/>
    public Task<FixerResult> FixAsync(IAnalyzer analyzer, IEnumerable<DirectoryInfo> directories,
        CancellationToken cancellationToken = default) =>
        FixAsync(new[] { analyzer }, directories, cancellationToken);

    /// <inheritdoc/>
    public Task<FixerResult> FixAsync(IEnumerable<IAnalyzer> analyzers,
        IEnumerable<DirectoryInfo> directories, CancellationToken cancellationToken = default)
    {
        var files = directories.SelectMany(d => d.EnumerateFiles(DefaultSearchPattern, new EnumerationOptions
        {
            RecurseSubdirectories = true,
        }));

        return FixAsync(analyzers, files, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<FixerResult> FixAsync(IEnumerable<IAnalyzer> analyzers, IEnumerable<FileInfo> files,
        CancellationToken cancellationToken = default)
    {
        var fixerList = analyzers.ToList();

        return await files.ToAsyncEnumerable().AggregateAwaitAsync(FixerResult.None,
            async (total, file) => total + await FixAsync(fixerList, file, cancellationToken),
            cancellationToken
        );
    }

    /// <inheritdoc/>
    public async Task<FixerResult> FixAsync(IEnumerable<IAnalyzer> analyzers, FileInfo file,
        CancellationToken cancellationToken = default)
    {
        return await analyzers.ToAsyncEnumerable().AggregateAwaitAsync(FixerResult.None,
            async (total, fixer) => total + await FixAsync(fixer, file, cancellationToken),
            cancellationToken
        );
    }

    /// <inheritdoc/>
    public event EventHandler<BeforeFixerRanEventArgs>? OnCheck;

    /// <inheritdoc/>
    public event EventHandler<AfterFixerRanEventArgs>? OnFixed;

    /// <inheritdoc/>
    public virtual async Task<FixerResult> FixAsync(IAnalyzer analyzer, FileInfo file,
        CancellationToken cancellationToken = default)
    {
        OnCheck?.Invoke(this, new(file, analyzer));

        Stopwatch sw = new();

        sw.Start();

        AnalysisResult analysisResult;
        try
        {
            analysisResult = analyzer switch
            {
                IStringAnalyzer stringFixer => await RunStringAnalyzer(stringFixer, file, cancellationToken),
                IFileAnalyzer fileFixer => await RunFileAnalyzer(fileFixer, file, cancellationToken),
                _ => throw new NotSupportedException($"Unknown analyzer type '{analyzer.GetType().FullName}'"),
            };

            sw.Stop();
        }
        catch (Exception e)
        {
            sw.Stop();

            if (ThrowOnFailure)
                throw new FixerException(analyzer, $"Failed to run analyzer '{analyzer.Name}' on file '{file.FullName}'", e);

            return FixerResult.Applied(0, sw.Elapsed);
        }

        var runDuration = sw.Elapsed;

        if (!analysisResult.FixRequired)
            return FixerResult.Applied(0, runDuration);

        var fixApplicatorResult = await ApplyResult(analysisResult, file, cancellationToken);

        OnFixed?.Invoke(this, new(file, analyzer));

        return fixApplicatorResult with { Elapsed = fixApplicatorResult.Elapsed + runDuration };
    }

    private static Task<FileAnalysisResult> RunFileAnalyzer(IFileAnalyzer analyzer, FileInfo file, CancellationToken cancellationToken)
    {
        return analyzer.AnalyzeAsync(file, cancellationToken);
    }

    private static async Task<StringAnalysisResult> RunStringAnalyzer(IStringAnalyzer analyzer, FileSystemInfo file, CancellationToken cancellationToken)
    {
        var contents = await File.ReadAllTextAsync(file.FullName, cancellationToken);

        return await analyzer.AnalyzeAsync(contents, cancellationToken);
    }

    /// <summary>
    /// Applies the result of an analysis to a file.
    /// </summary>
    /// <param name="result">The result of the analysis.</param>
    /// <param name="file">The file to apply the result to.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the fixer.</returns>
    protected abstract Task<FixerResult> ApplyResult(AnalysisResult result, FileInfo file, CancellationToken cancellationToken);
}