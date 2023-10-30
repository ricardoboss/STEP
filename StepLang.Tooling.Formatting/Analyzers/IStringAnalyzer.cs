using StepLang.Tooling.Formatting.Analyzers.Results;

namespace StepLang.Tooling.Formatting.Analyzers;

/// <summary>
/// An analyzer that can be used to analyze source code.
/// </summary>
public interface IStringAnalyzer : IAnalyzer
{
    /// <summary>
    /// Applies a fix to the input string.
    /// </summary>
    /// <param name="input">A string to apply the fix to.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A result containing a string with the applied fix.</returns>
    Task<StringAnalysisResult> AnalyzeAsync(string input, CancellationToken cancellationToken = default);
}