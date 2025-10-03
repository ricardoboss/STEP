namespace StepLang.Tooling.Formatting.Analyzers.Results;

public interface IAnalysisResult
{
	bool ShouldFix { get; }
}
