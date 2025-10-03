using StepLang.Tooling.Formatting.Analyzers.Results;

namespace StepLang.Tooling.Formatting.Analyzers;

public class TrailingWhitespaceAnalyzer : IStringAnalyzer
{
	public Task<StringAnalysisResult> AnalyzeAsync(string input, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();

		var fixedString = input.WithoutTrailingWhitespace();

		return Task.FromResult(StringAnalysisResult.FromInputAndFix(AnalysisSeverity.Suggestion, input, fixedString));
	}
}
