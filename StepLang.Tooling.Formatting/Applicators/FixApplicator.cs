using StepLang.Tooling.Formatting.Fixers;

namespace StepLang.Tooling.Formatting.Applicators;

public interface IFixApplicator
{
    public event EventHandler<BeforeFixerRanEventArgs>? BeforeFixerRun;

    public event EventHandler<AfterFixerRanEventArgs>? AfterApplyFix;

    public bool ThrowOnFailure { get; init; }

    public Task<FixApplicatorResult> ApplyFixesAsync(IFixer fixer, FileInfo file, CancellationToken cancellationToken = default);
    public Task<FixApplicatorResult> ApplyFixesAsync(IEnumerable<IFixer> fixers, FileInfo file, CancellationToken cancellationToken = default);
    public Task<FixApplicatorResult> ApplyFixesAsync(IFixer fixer, DirectoryInfo directory, CancellationToken cancellationToken = default);
    public Task<FixApplicatorResult> ApplyFixesAsync(IEnumerable<IFixer> fixers, DirectoryInfo directory, CancellationToken cancellationToken = default);
    public Task<FixApplicatorResult> ApplyFixesAsync(IFixer fixer, IEnumerable<FileInfo> files, CancellationToken cancellationToken = default);
    public Task<FixApplicatorResult> ApplyFixesAsync(IEnumerable<IFixer> fixers, IEnumerable<FileInfo> files, CancellationToken cancellationToken = default);
    public Task<FixApplicatorResult> ApplyFixesAsync(IFixer fixer, IEnumerable<DirectoryInfo> directories, CancellationToken cancellationToken = default);
    public Task<FixApplicatorResult> ApplyFixesAsync(IEnumerable<IFixer> fixers, IEnumerable<DirectoryInfo> directories, CancellationToken cancellationToken = default);
}