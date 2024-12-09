using StepLang.Tooling.Formatting.Analyzers;

namespace StepLang.Tooling.Formatting.Fixers;

public interface IFixer
{
	public event EventHandler<BeforeFixerRanEventArgs>? OnCheck;

	public event EventHandler<AfterFixerRanEventArgs>? OnFixed;

	public bool ThrowOnFailure { get; init; }

	public Task<FixerResult> FixAsync(IAnalyzer analyzer, FileInfo file, CancellationToken cancellationToken = default);

	public Task<FixerResult> FixAsync(IEnumerable<IAnalyzer> analyzers, FileInfo file,
		CancellationToken cancellationToken = default);

	public Task<FixerResult> FixAsync(IAnalyzer analyzer, DirectoryInfo directory,
		CancellationToken cancellationToken = default);

	public Task<FixerResult> FixAsync(IEnumerable<IAnalyzer> analyzers, DirectoryInfo directory,
		CancellationToken cancellationToken = default);

	public Task<FixerResult> FixAsync(IAnalyzer analyzer, IEnumerable<FileInfo> files,
		CancellationToken cancellationToken = default);

	public Task<FixerResult> FixAsync(IEnumerable<IAnalyzer> analyzers, IEnumerable<FileInfo> files,
		CancellationToken cancellationToken = default);

	public Task<FixerResult> FixAsync(IAnalyzer analyzer, IEnumerable<DirectoryInfo> directories,
		CancellationToken cancellationToken = default);

	public Task<FixerResult> FixAsync(IEnumerable<IAnalyzer> analyzers, IEnumerable<DirectoryInfo> directories,
		CancellationToken cancellationToken = default);
}
