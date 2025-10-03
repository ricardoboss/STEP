using StepLang.Tooling.Formatting.Analyzers.Results;
using StepLang.Tooling.Formatting.Analyzers.Source;
using System.Diagnostics;

namespace StepLang.Tooling.Formatting.Fixers;

public class DefaultFixer : BaseFixer
{
	protected override async Task<FixerResult> ApplyResultAsync(IApplicableAnalysisResult result, IFixerSource source,
		CancellationToken cancellationToken)
	{
		Stopwatch sw = new();

		sw.Start();

		await result.ApplyFixAsync(source, cancellationToken);

		sw.Stop();

		return FixerResult.Applied(sw.Elapsed);
	}
}
