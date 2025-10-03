using StepLang.Tooling.Formatting.Analyzers;
using StepLang.Tooling.Formatting.Analyzers.Source;

namespace StepLang.Tooling.Formatting.Fixers;

public interface IFixer
{
	event EventHandler<BeforeFixerRanEventArgs>? OnCheck;

	event EventHandler<UnfixableEventArgs>? OnUnfixable;

	event EventHandler<AfterFixerRanEventArgs>? OnFixed;

	bool ThrowOnFailure { get; init; }

	Task<FixerResult> FixAsync(IEnumerable<IAnalyzer> analyzers, IFixerSource source,
		CancellationToken cancellationToken = default);

	Task<FixerResult> FixAsync(IEnumerable<IAnalyzer> analyzers, FileInfo file,
		CancellationToken cancellationToken = default);

	Task<FixerResult> FixAsync(IEnumerable<IAnalyzer> analyzers, DirectoryInfo directory,
		CancellationToken cancellationToken = default);
}
