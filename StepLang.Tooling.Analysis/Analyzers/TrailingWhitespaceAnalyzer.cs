using StepLang.Tooling.Analysis.Analyzers.Results;

namespace StepLang.Tooling.Analysis.Analyzers;

public class TrailingWhitespaceAnalyzer : IStringAnalyzer
{
	public Task<StringAnalysisResult> AnalyzeAsync(string input, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();

		var fixedString = input.WithoutTrailingWhitespace();

		return Task.FromResult(StringAnalysisResult.FromInputAndFix(input, fixedString));
	}

	public string Name => "Trailing Whitespace";
}
