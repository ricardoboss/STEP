namespace StepLang.Tooling.Formatting.Analyzers.Results;

public interface IAnalysisResult
{
	AnalysisSeverity Severity { get; }

	bool ShouldFix { get; }
}
