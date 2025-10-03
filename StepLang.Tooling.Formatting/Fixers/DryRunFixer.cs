using StepLang.Tooling.Formatting.Analyzers.Results;
using StepLang.Tooling.Formatting.Analyzers.Source;

namespace StepLang.Tooling.Formatting.Fixers;

public class DryRunFixer : BaseFixer
{
	protected override Task<FixerResult> ApplyResultAsync(IApplicableAnalysisResult result, IFixerSource source,
		CancellationToken cancellationToken)
	{
		// act as if a fix was applied to count number of potential fixes
		return Task.FromResult(FixerResult.Applied(TimeSpan.Zero));
	}
}
