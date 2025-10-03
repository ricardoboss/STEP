using StepLang.Tooling.Formatting.Analyzers.Results;
using StepLang.Tooling.Formatting.Analyzers.Source;

namespace StepLang.Tooling.Formatting.Analyzers;

public interface IStringAnalyzer : IAnalyzer
{
	/// <summary>
	/// Applies a fix to the input string.
	/// </summary>
	/// <param name="input">A string to apply the fix to.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A result containing a string with the applied fix.</returns>
	Task<StringAnalysisResult> AnalyzeAsync(string input, CancellationToken cancellationToken = default);

	/// <inheritdoc />
	async Task<IAnalysisResult> IAnalyzer.AnalyzeAsync(IFixerSource source, CancellationToken cancellationToken)
	{
		var contents = await source.GetSourceCodeAsync(cancellationToken);

		return await AnalyzeAsync(contents, cancellationToken);
	}
}
