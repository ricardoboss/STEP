using StepLang.Tooling.Formatting.Analyzers.Results;

namespace StepLang.Tooling.Formatting.Analyzers;

/// <summary>
/// Removes trailing whitespace from the input string.
/// </summary>
public class TrailingWhitespaceAnalyzer : IStringAnalyzer
{
    /// <inheritdoc/>
    public Task<StringAnalysisResult> AnalyzeAsync(string input, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var fixedString = input.WithoutTrailingWhitespace();

        return Task.FromResult(StringAnalysisResult.FromInputAndFix(input, fixedString));
    }
}