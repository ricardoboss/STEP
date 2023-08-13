namespace StepLang.Formatters;

public interface IFixApplicator
{
    public Task ApplyFixesAsync(IFixer fixer, FileInfo file, CancellationToken cancellationToken = default);

    public Task ApplyFixesAsync(IEnumerable<IFixer> fixers, FileInfo file, CancellationToken cancellationToken = default);

    public Task ApplyFixesAsync(IFixer fixer, DirectoryInfo directory, CancellationToken cancellationToken = default);

    public Task ApplyFixesAsync(IEnumerable<IFixer> fixers, DirectoryInfo directory, CancellationToken cancellationToken = default);

    public Task ApplyFixesAsync(IFixer fixer, IEnumerable<FileInfo> files, CancellationToken cancellationToken = default);

    public Task ApplyFixesAsync(IEnumerable<IFixer> fixers, IEnumerable<FileInfo> files, CancellationToken cancellationToken = default);

    public Task ApplyFixesAsync(IFixer fixer, IEnumerable<DirectoryInfo> directories, CancellationToken cancellationToken = default);

    public Task ApplyFixesAsync(IEnumerable<IFixer> fixers, IEnumerable<DirectoryInfo> directories, CancellationToken cancellationToken = default);
}