using StepLang.Tooling.Analysis.Analyzers.Source;

namespace StepLang.Tooling.Analysis.Analyzers.Results;

public interface IApplicableAnalysisResult : IAnalysisResult
{
	bool FixAvailable { get; }

	Task ApplyFixAsync(IFixerSource source, CancellationToken cancellationToken = default);
}
