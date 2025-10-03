using StepLang.Tooling.Formatting.Analyzers.Results;
using StepLang.Tooling.Formatting.Analyzers.Source;

namespace StepLang.Tooling.Formatting.Analyzers;

/// <summary>
/// This is just a marker for all fixers.
/// </summary>
public interface IAnalyzer
{
	string Name => GetType().Name;

	/// <summary>
	/// Analyzes the given <paramref name="source"/> and returns a result detailing whether a fix is required and
	/// optionally providing a fixed version of the source.
	/// </summary>
	/// <param name="source">An accessor for the source code and, if applicable, the source file.</param>
	/// <param name="cancellationToken">A token to cancel the asynchronous task.</param>
	/// <returns>An <see cref="IAnalysisResult"/> detailing whether a fix is required.</returns>
	Task<IAnalysisResult> AnalyzeAsync(IFixerSource source, CancellationToken cancellationToken = default);
}
