namespace StepLang.Formatters;

public abstract class BaseFixApplicator : IFixApplicator
{
    private const string DefaultSearchPattern = "*.step";

    public abstract bool ThrowOnFailure { get; init; }

    public abstract bool DryRun { get; init; }

    public async Task<FixApplicatorResult> ApplyFixesAsync(IFixer fixer, DirectoryInfo directory, CancellationToken cancellationToken = default) => await ApplyFixesAsync(new [] { fixer }, new []{ directory }, cancellationToken);

    public async Task<FixApplicatorResult> ApplyFixesAsync(IEnumerable<IFixer> fixers, DirectoryInfo directory, CancellationToken cancellationToken = default) => await ApplyFixesAsync(fixers, new []{ directory }, cancellationToken);

    public async Task<FixApplicatorResult> ApplyFixesAsync(IFixer fixer, IEnumerable<FileInfo> files, CancellationToken cancellationToken = default) => await ApplyFixesAsync(new [] { fixer }, files, cancellationToken);

    public async Task<FixApplicatorResult> ApplyFixesAsync(IFixer fixer, IEnumerable<DirectoryInfo> directories, CancellationToken cancellationToken = default) => await ApplyFixesAsync(new [] { fixer }, directories, cancellationToken);

    public async Task<FixApplicatorResult> ApplyFixesAsync(IEnumerable<IFixer> fixers, IEnumerable<DirectoryInfo> directories, CancellationToken cancellationToken = default)
    {
        var files = directories.SelectMany(d => d.EnumerateFiles(DefaultSearchPattern, new EnumerationOptions
        {
            RecurseSubdirectories = true,
        }));

        return await ApplyFixesAsync(fixers, files, cancellationToken);
    }

    public async Task<FixApplicatorResult> ApplyFixesAsync(IEnumerable<IFixer> fixers, IEnumerable<FileInfo> files, CancellationToken cancellationToken = default)
    {
        var fixerList = fixers.ToList();
        var results = new List<FixApplicatorResult>();

        foreach (var file in files)
            results.Add(await ApplyFixesAsync(fixerList, file, cancellationToken));

        return results.Aggregate(FixApplicatorResult.Zero, (total, result) => total + result);
    }

    public async Task<FixApplicatorResult> ApplyFixesAsync(IEnumerable<IFixer> fixers, FileInfo file, CancellationToken cancellationToken = default)
    {
        var results = new List<FixApplicatorResult>();
        foreach (var fixer in fixers)
            results.Add(await ApplyFixesAsync(fixer, file, cancellationToken));

        return results.Aggregate(FixApplicatorResult.Zero, (total, result) => total + result);
    }

    public abstract Task<FixApplicatorResult> ApplyFixesAsync(IFixer fixer, FileInfo file, CancellationToken cancellationToken = default);
}