using StepLang.Tooling.Formatting.Analyzers.Results;

namespace StepLang.Tooling.Formatting.Fixers;

public class DryRunFixer : BaseFixer
{
	protected override Task<FixerResult> ApplyResult(AnalysisResult result, FileInfo file,
		CancellationToken cancellationToken)
	{
		return Task.FromResult(FixerResult.Applied(1, TimeSpan.Zero));
	}
}
