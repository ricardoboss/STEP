using System.Diagnostics;
using StepLang.Tooling.Formatting.Fixers;
using StepLang.Tooling.Formatting.Fixers.Results;

namespace StepLang.Tooling.Formatting.Applicators;

public abstract class BaseFixApplicator : IFixApplicator
{
    private const string DefaultSearchPattern = "*.step";

    public virtual bool ThrowOnFailure { get; init; } = true;

    public virtual FixerConfiguration Configuration { get; init; } = new();

    public async Task<FixApplicatorResult> ApplyFixesAsync(IFixer fixer, DirectoryInfo directory,
        CancellationToken cancellationToken = default) =>
        await ApplyFixesAsync(new [] { fixer }, new [] { directory }, cancellationToken);

    public async Task<FixApplicatorResult> ApplyFixesAsync(IEnumerable<IFixer> fixers, DirectoryInfo directory,
        CancellationToken cancellationToken = default) =>
        await ApplyFixesAsync(fixers, new [] { directory }, cancellationToken);

    public async Task<FixApplicatorResult> ApplyFixesAsync(IFixer fixer, IEnumerable<FileInfo> files,
        CancellationToken cancellationToken = default) =>
        await ApplyFixesAsync(new [] { fixer }, files, cancellationToken);

    public async Task<FixApplicatorResult> ApplyFixesAsync(IFixer fixer, IEnumerable<DirectoryInfo> directories,
        CancellationToken cancellationToken = default) =>
        await ApplyFixesAsync(new [] { fixer }, directories, cancellationToken);

    public async Task<FixApplicatorResult> ApplyFixesAsync(IEnumerable<IFixer> fixers,
        IEnumerable<DirectoryInfo> directories, CancellationToken cancellationToken = default)
    {
        var files = directories.SelectMany(d => d.EnumerateFiles(DefaultSearchPattern, new EnumerationOptions
        {
            RecurseSubdirectories = true,
        }));

        return await ApplyFixesAsync(fixers, files, cancellationToken);
    }

    public async Task<FixApplicatorResult> ApplyFixesAsync(IEnumerable<IFixer> fixers, IEnumerable<FileInfo> files,
        CancellationToken cancellationToken = default)
    {
        var fixerList = fixers.ToList();

        return await files.ToAsyncEnumerable().AggregateAwaitAsync(FixApplicatorResult.Zero,
            async (total, file) => total + await ApplyFixesAsync(fixerList, file, cancellationToken),
            cancellationToken
        );
    }

    public async Task<FixApplicatorResult> ApplyFixesAsync(IEnumerable<IFixer> fixers, FileInfo file,
        CancellationToken cancellationToken = default)
    {
        return await fixers.ToAsyncEnumerable().AggregateAwaitAsync(FixApplicatorResult.Zero,
            async (total, fixer) => total + await ApplyFixesAsync(fixer, file, cancellationToken),
            cancellationToken
        );
    }

    public virtual event EventHandler<BeforeFixerRanEventArgs>? BeforeFixerRun;

    public virtual event EventHandler<AfterFixerRanEventArgs>? AfterApplyFix;

    public virtual async Task<FixApplicatorResult> ApplyFixesAsync(IFixer fixer, FileInfo file,
        CancellationToken cancellationToken = default)
    {
        BeforeFixerRun?.Invoke(this, new(file, fixer));

        Stopwatch sw = new();

        sw.Start();

        FixResult fixResult;
        try
        {
            fixResult = fixer switch
            {
                IStringFixer stringFixer => await RunStringFixer(stringFixer, file, cancellationToken),
                IFileFixer fileFixer => await RunFileFixer(fileFixer, file, cancellationToken),
                _ => throw new NotImplementedException($"Unknown fixer type '{fixer.GetType().FullName}'"),
            };

            sw.Stop();
        }
        catch (Exception e)
        {
            sw.Stop();

            if (ThrowOnFailure)
                throw new FixerException(fixer, $"Failed to run fixer '{fixer.Name}' on file '{file.FullName}'", e);

            return new(0, 1, sw.Elapsed);
        }

        var runDuration = sw.Elapsed;

        if (!fixResult.FixRequired)
            return new(0, 0, runDuration);

        var fixApplicatorResult = await ApplyResult(fixResult, file, cancellationToken);

        AfterApplyFix?.Invoke(this, new(file, fixer));

        return fixApplicatorResult with { Elapsed = fixApplicatorResult.Elapsed + runDuration };
    }

    private async Task<FileFixResult> RunFileFixer(IFileFixer fixer, FileInfo file, CancellationToken cancellationToken)
    {
        return await fixer.FixAsync(file, Configuration, cancellationToken);
    }

    private async Task<StringFixResult> RunStringFixer(IStringFixer fixer, FileSystemInfo file, CancellationToken cancellationToken)
    {
        var contents = await File.ReadAllTextAsync(file.FullName, cancellationToken);

        return await fixer.FixAsync(contents, Configuration, cancellationToken);
    }

    protected abstract Task<FixApplicatorResult> ApplyResult(FixResult result, FileInfo file, CancellationToken cancellationToken);
}