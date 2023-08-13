namespace StepLang.Formatters;

public abstract class BaseFixApplicator : IFixApplicator
{
    private const string DefaultSearchPattern = "*.step";

    public async Task ApplyFixesAsync(IFixer fixer, DirectoryInfo directory, CancellationToken cancellationToken = default) => await ApplyFixesAsync(new [] { fixer }, new []{ directory }, cancellationToken);

    public async Task ApplyFixesAsync(IEnumerable<IFixer> fixers, DirectoryInfo directory, CancellationToken cancellationToken = default) => await ApplyFixesAsync(fixers, new []{ directory }, cancellationToken);

    public async Task ApplyFixesAsync(IFixer fixer, IEnumerable<FileInfo> files, CancellationToken cancellationToken = default) => await ApplyFixesAsync(new [] { fixer }, files, cancellationToken);

    public async Task ApplyFixesAsync(IFixer fixer, IEnumerable<DirectoryInfo> directories, CancellationToken cancellationToken = default) => await ApplyFixesAsync(new [] { fixer }, directories, cancellationToken);

    public async Task ApplyFixesAsync(IEnumerable<IFixer> fixers, IEnumerable<DirectoryInfo> directories, CancellationToken cancellationToken = default)
    {
        var files = directories.SelectMany(d => d.EnumerateFiles(DefaultSearchPattern, new EnumerationOptions
        {
            RecurseSubdirectories = true,
        }));

        await ApplyFixesAsync(fixers, files, cancellationToken);
    }

    public async Task ApplyFixesAsync(IEnumerable<IFixer> fixers, IEnumerable<FileInfo> files, CancellationToken cancellationToken = default)
    {
        var fixerList = fixers.ToList();

        foreach (var file in files)
            await ApplyFixesAsync(fixerList, file, cancellationToken);
    }

    public async Task ApplyFixesAsync(IEnumerable<IFixer> fixers, FileInfo file, CancellationToken cancellationToken = default)
    {
        foreach (var fixer in fixers)
            await ApplyFixesAsync(fixer, file, cancellationToken);
    }

    public abstract Task ApplyFixesAsync(IFixer fixer, FileInfo file, CancellationToken cancellationToken = default);
}