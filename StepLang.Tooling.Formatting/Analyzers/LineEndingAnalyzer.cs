using StepLang.Tooling.Formatting.Analyzers.Results;

namespace StepLang.Tooling.Formatting.Analyzers;

public class LineEndingAnalyzer : IStringAnalyzer
{
	private const string DefaultLineEnding = "\n";

	public Task<StringAnalysisResult> AnalyzeAsync(string input, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();

		var fixedString = string.Join(DefaultLineEnding, input.SplitLines());

		return Task.FromResult(StringAnalysisResult.FromInputAndFix(AnalysisSeverity.Suggestion, input, fixedString));
	}
}
