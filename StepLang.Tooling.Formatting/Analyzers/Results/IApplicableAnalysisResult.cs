using StepLang.Tooling.Formatting.Analyzers.Source;

namespace StepLang.Tooling.Formatting.Analyzers.Results;

public interface IApplicableAnalysisResult : IAnalysisResult
{
	bool FixAvailable { get; }

	Task ApplyFixAsync(IFixerSource source, CancellationToken cancellationToken = default);
}
